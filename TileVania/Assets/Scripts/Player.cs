using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float playerRunVelocity = 5f;
    [SerializeField] float playerJumpVelocity = 5f;
    [SerializeField] float playerClimbVelocity = 5f;
    [SerializeField] float playerClimbHorizontalVelocity = 1f;
    [SerializeField] float ladderGravityScale = 0f;

    // Cached References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;
    float gravityScaleAtStart;

    // State Variables
    [SerializeField] bool isRunning = false;
    [SerializeField] bool isClimbing = false;
    [SerializeField] bool isInAir = false;

    bool jumpedFromBottomOfLadder = false;
    bool standingAtTopOfLadder = false;
    bool standingAtBottomOfLadder = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();

        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimations();

        Run();

        ClimbLadder();

        CheckJump();

        HandleSpriteFlipping();
    }

    private void HandleAnimations()
    {
        myAnimator.SetBool("isClimbing", isClimbing);

        myAnimator.SetBool("isRunning", isRunning);
    }

    private void Run()
    {
        float currentInput = Input.GetAxis("Horizontal");

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
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isInAir = true;
            return;
        }
        else
        {
            isInAir = false;
        }

        if (Input.GetButtonDown("Jump") && !isClimbing && !jumpedFromBottomOfLadder)
        {
            Jump();
        }
    }

    private void Jump()
    {
        Vector2 newVelocityToAdd = new Vector2(0f, playerJumpVelocity);

        myRigidBody.velocity += newVelocityToAdd;

        isInAir = true;
    }

    private void ClimbLadder()
    {
        // See if standing at the top of the ladder
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("LadderTops")) && !standingAtTopOfLadder && isClimbing)
        {
            standingAtTopOfLadder = true;

            myRigidBody.gravityScale = 0f;
            myRigidBody.velocity = new Vector2(0f, 0f);

            isClimbing = false;
            myAnimator.SetBool("isClimbing", isClimbing);

            return;
        }
        
        // If we're at the top, "leave" the top if we are no longer touching the collider
        if (standingAtTopOfLadder && !myCollider.IsTouchingLayers(LayerMask.GetMask("LadderTops")))
        {
            standingAtTopOfLadder = false;
            myRigidBody.gravityScale = gravityScaleAtStart;
        }

        // If we're standing at the top, check if we're trying to move down back onto the ladder
        if (standingAtTopOfLadder)
        {

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
            isClimbing && !standingAtBottomOfLadder && myCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")) 
            && myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && Input.GetAxis("Vertical") < 0f 
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
            if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")) || Input.GetAxis("Vertical") > 0f)
            {
                standingAtBottomOfLadder = false;
            }

            return;
        }

        // If we aren't touching a ladder, don't do anything
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
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

            jumpedFromBottomOfLadder = true;

            StartCoroutine(WaitAndResetJumpedFromBottomOfLadder());
        }
    }

    private IEnumerator WaitAndResetJumpedFromBottomOfLadder()
    {
        yield return new WaitForSeconds(0.2f);

        jumpedFromBottomOfLadder = false;
    }

    private void HandleSpriteFlipping()
    {
        if (isRunning)
        {
            float playerDirection = Mathf.Sign(myRigidBody.velocity.x);
            
            transform.localScale = new Vector2(playerDirection, 1f);
        }
    }
}
