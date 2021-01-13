using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Config Parameters
    [Header("Player Chooser")]
    [SerializeField] int playerNumber = 0;

    [Header("Player Movement")]
    [SerializeField] float playerRunVelocity = 5f;
    [SerializeField] float playerJumpVelocity = 5f;
    [SerializeField] float playerClimbVelocity = 5f;
    [SerializeField] float playerClimbHorizontalVelocity = 1f;
    [SerializeField] float ladderGravityScale = 0f;

    [Header("Player Lives/Death")]
    [SerializeField] int playerHealth = 1;
    [SerializeField] float damageDelay = 1f;
    [SerializeField] float playerDeathLaunchVelocity = 20f;

    [Header("Audio Clips")]
    [SerializeField] AudioClip runSound = null;
    [SerializeField] AudioClip jumpSound = null;
    [SerializeField] AudioClip climbSound = null;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip deathSound = null;

    // Cached References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    GameSession gameSession;
    AudioManager audioManager;
    PlayerChooser playerChooser;

    float gravityScaleAtStart;

    // State Variables
    bool isRunning = false;
    bool isClimbing = false;
    bool isInAir = false;

    bool isDead = false;
    bool isTimerUp = true;

    bool isExitingRoom = false;

    bool isPaused = false;

    bool jumpedFromBottomOrTopOfLadder = false;
    bool standingAtTopOfLadder = false;
    bool standingAtBottomOfLadder = false;

    bool gameSessionFound = false;

    bool isActivePlayerNumber = false;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        playerChooser = FindObjectOfType<PlayerChooser>();
        audioManager = FindObjectOfType<AudioManager>();

        gravityScaleAtStart = myRigidBody.gravityScale;

        if (FindObjectOfType<GameSession>())
        {
            FindObjectOfType<GameSession>().UpdateUI();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (playerChooser.GetActivePlayerNumber() == playerNumber)
            {
                isActivePlayerNumber = true;
            }
        }
        else
        {
            if (FindObjectsOfType<GameSession>().Length > 1)
            {
                isActivePlayerNumber = (FindObjectsOfType<GameSession>()[1].GetActivePlayerNumber() == playerNumber);
            }
            else
            {
                isActivePlayerNumber = (FindObjectOfType<GameSession>().GetActivePlayerNumber() == playerNumber);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivePlayerNumber)
        {
            if (!isPaused)
            {
                CheckRestart();

                HandleAnimations();

                if (!isDead)
                {
                    CheckDamaged();

                    Run();

                    ClimbLadder();

                    CheckJump();

                    HandleSpriteFlipping();
                }
            }
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    private void CheckDamaged()
    {
        if (isTimerUp)
        {
            if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"))
                || myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
            {
                PlayHitSound();

                if (!gameSessionFound)
                {
                    gameSession = FindObjectOfType<GameSession>();

                    gameSessionFound = true;
                }

                playerHealth--;

                gameSession.UpdateUI();

                if (playerHealth <= 0)
                {
                    Die();
                }
                else
                {
                    StartCoroutine(DeathDetectionTimeout());
                }
            }
        }
    }

    private void Die()
    {
        PlayDeathSound();

        Vector2 deathVelocity = new Vector2(Random.Range(-0.2f, 0.2f), 1f) * playerDeathLaunchVelocity;

        myRigidBody.velocity = deathVelocity;

        isDead = true;
        
        gameSession.ProcessPlayerDeath();
    }

    private void CheckRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }

    private void HandleAnimations()
    {
        myAnimator.SetBool("isDead", isDead);

        myAnimator.SetBool("isClimbing", isClimbing);

        myAnimator.SetBool("isRunning", isRunning);
    }

    private void Run()
    {
        float currentInput = Input.GetAxisRaw("Horizontal");

        float currentVelocity = playerRunVelocity;
        if (isClimbing)
        {
            currentVelocity = playerClimbHorizontalVelocity;
        }

        Vector2 newVelocity = new Vector2(currentVelocity * currentInput, myRigidBody.velocity.y);

        myRigidBody.velocity = newVelocity;

        isRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    private void CheckJump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"))
             && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isInAir = true;
            return;
        }
        else
        {
            isInAir = false;
        }

        if (Input.GetButtonDown("Jump") && !isClimbing && !jumpedFromBottomOrTopOfLadder)
        {
            Jump();
        }
    }

    private void Jump()
    {
        PlayJumpSound();

        Vector2 newVelocityToAdd = new Vector2(0f, playerJumpVelocity);

        myRigidBody.velocity += newVelocityToAdd;

        isInAir = true;
    }

    private void ClimbLadder()
    {
        // See if standing at the top of the ladder
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("LadderTops")) && !standingAtTopOfLadder && isClimbing)
        {
            standingAtTopOfLadder = true;

            myRigidBody.gravityScale = 0f;
            myRigidBody.velocity = new Vector2(0f, 0f);

            isClimbing = false;
            myAnimator.SetBool("isClimbing", isClimbing);

            return;
        }
        
        // If we're at the top, "leave" the top if we are no longer touching the collider
        if (standingAtTopOfLadder && !myBodyCollider.IsTouchingLayers(LayerMask.GetMask("LadderTops")))
        {
            standingAtTopOfLadder = false;
            myRigidBody.gravityScale = gravityScaleAtStart;
        }

        // If we're standing at the top, check if we're trying to move down back onto the ladder, or trying to jump off the top
        if (standingAtTopOfLadder)
        {
            if (Input.GetButtonDown("Jump"))
            {
                myRigidBody.gravityScale = gravityScaleAtStart;

                Jump();

                jumpedFromBottomOrTopOfLadder = true;

                StartCoroutine(WaitAndResetJumpedFromBottomOrTopOfLadder());

                return;
            }

            float topInput = Input.GetAxis("Vertical");

            if (topInput < 0)
            {
                myAnimator.enabled = true;

                Vector2 newVelocity = new Vector2(myRigidBody.velocity.x, playerClimbVelocity * topInput);

                myRigidBody.velocity = newVelocity;

                myRigidBody.gravityScale = ladderGravityScale;

                isClimbing = true;
            }

            return;
        }

        // Check if we are at the bottom of the ladder, and continue moving down to get off.
        if (
            isClimbing && !standingAtBottomOfLadder && myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")) 
            && myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && Input.GetAxis("Vertical") < 0f 
            && Mathf.Abs(myRigidBody.velocity.y) < Mathf.Epsilon
            )
        {
            standingAtBottomOfLadder = true;

            myRigidBody.gravityScale = gravityScaleAtStart;

            isClimbing = false;
            myAnimator.SetBool("isClimbing", isClimbing);

            return;
        }

        // If we are standing at the bottom of the ladder, get back on if we're moving up, or move away if we are no longer touching the ladder
        if (standingAtBottomOfLadder)
        {
            if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")) || Input.GetAxis("Vertical") > 0f)
            {
                standingAtBottomOfLadder = false;
            }

            return;
        }

        // If we aren't touching a ladder, don't do anything
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myAnimator.enabled = true;

            isClimbing = false;

            myRigidBody.gravityScale = gravityScaleAtStart;

            return;
        }
    
        // Move up and down the ladder if we are touching it, and using vertical keys
        float currentInput = Input.GetAxis("Vertical");

        if (currentInput != 0)
        {
            myAnimator.enabled = true;

            Vector2 newVelocity = new Vector2(myRigidBody.velocity.x, playerClimbVelocity * currentInput);

            myRigidBody.velocity = newVelocity;

            myRigidBody.gravityScale = ladderGravityScale;

            isClimbing = true;
        }
        else
        {
            if (isClimbing)
            {
                myAnimator.enabled = false;

                Vector2 newVelocity = new Vector2(myRigidBody.velocity.x, 0f);

                myRigidBody.velocity = newVelocity;
            }
        }

        // Let us jump while on the ladder!
        if (Input.GetButtonDown("Jump") && isClimbing)
        {
            myAnimator.enabled = true;

            myRigidBody.gravityScale = gravityScaleAtStart;

            Jump();

            isClimbing = false;
            myAnimator.SetBool("isClimbing", isClimbing);

            jumpedFromBottomOrTopOfLadder = true;

            StartCoroutine(WaitAndResetJumpedFromBottomOrTopOfLadder());
        }
    }

    private IEnumerator WaitAndResetJumpedFromBottomOrTopOfLadder()
    {
        yield return new WaitForSeconds(0.2f);

        jumpedFromBottomOrTopOfLadder = false;
    }

    private IEnumerator DeathDetectionTimeout()
    {
        myAnimator.SetBool("isHurt", true);

        isTimerUp = false;

        yield return new WaitForSeconds(damageDelay);

        isTimerUp = true;
        
        myAnimator.SetBool("isHurt", false);
        myAnimator.SetTrigger("backToIdle");
    }

    private void HandleSpriteFlipping()
    {
        if (isRunning)
        {
            float playerDirection = Mathf.Sign(myRigidBody.velocity.x);
            
            transform.localScale = new Vector2(playerDirection, 1f);
        }
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public void PauseControls()
    {
        isPaused = true;
    }

    public void UnpauseControls()
    {
        isPaused = false;
    }

    public void SetDoorWalkingAnimation(float timeToFade)
    {
        isRunning = false;
        isClimbing = true;

        myAnimator.SetBool("isRunning", isRunning);
        myAnimator.SetBool("isClimbing", isClimbing);

        StartCoroutine(FadeToDistance(timeToFade));
    }

    IEnumerator FadeToDistance(float timeToFade)
    {
        float startScale = gameObject.transform.localScale.x;
        float endScale = 0f;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / timeToFade)
        {
            Vector2 newScale = new Vector2( Mathf.Abs(Mathf.Lerp(startScale, endScale, t)), Mathf.Abs(Mathf.Lerp(startScale, endScale, t)) );

            gameObject.transform.localScale = newScale;

            yield return null;
        }
    }

    public void AddHealth(int amount)
    {
        playerHealth += amount;
        FindObjectOfType<GameSession>().UpdateUI();
    }

    public void SetExitingRoom()
    {
        isExitingRoom = true;
    }

    public void PlayFootstepSound()
    {
        if (!isInAir)
        {
            audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.runSound);
        }
    }

    public void PlayClimbingSound()
    {
        if (!isExitingRoom)
        {
            audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.climbSound);
        }
        else
        {
            audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.runSound);
        }
    }

    public void PlayHitSound()
    {
        audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.hitSound);
    }

    public void PlayJumpSound()
    {
        audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.jumpSound);
    }

    public void PlayDeathSound()
    {
        audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.deathSound);
    }

    public void IsActivePlayer(int activePlayerNumber)
    {
        if (activePlayerNumber == playerNumber)
        {
            isActivePlayerNumber = true;
        }
        else
        {
            isActivePlayerNumber = false;
        }
    }

    public bool AmIActive()
    {
        return isActivePlayerNumber;
    }
}
