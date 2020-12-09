using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Config parameters
    [SerializeField] Paddle paddle1;
    [SerializeField] float blackHoleRotationSpeed = 100f;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;

    [SerializeField] float ballDestroyDelay = 0.2f;

    [SerializeField] AudioClip[] ballSounds = null;

    [SerializeField] float randomFactor = 0.2f;

    // State
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached component references
    Rigidbody2D myRigidBody2D = null;
    AudioSource myAudioSource = null;

    // Use this for initialization
    void Start ()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();

        paddleToBallVector = transform.position - paddle1.transform.position;
    }
	
    public void DestroyBall()
    {
        Destroy(gameObject, ballDestroyDelay);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0, 0, blackHoleRotationSpeed * Time.deltaTime);

        if (!hasStarted)
        {
            LockToPaddle();
            LaunchOnClick();
        }
    }


    private void LaunchOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;

            myRigidBody2D.velocity = new Vector2(xPush, yPush);
        }
    }


    private void LockToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);

        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (Random.Range(0f, randomFactor), 
            Random.Range(0f, randomFactor));

        if (hasStarted)
        {
            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);

            myRigidBody2D.velocity += velocityTweak;
        }
    }
}
