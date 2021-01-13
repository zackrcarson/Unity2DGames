using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float walkSpeed = 1f;

    // Cached References
    Rigidbody2D myRigidBody;
    CapsuleCollider2D wallDetector;

    // State Variables
    bool isTimerUp = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        wallDetector = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        Vector2 newVelocity = new Vector2(0f, 0f);

        if (IsFacingRight())
        {
            newVelocity = new Vector2(walkSpeed, 0f);
        }
        else
        {
            newVelocity = new Vector2(-walkSpeed, 0f);
        }

        myRigidBody.velocity = newVelocity;
    }

    private bool IsFacingRight()
    {
        bool isFacingRight = (transform.localScale.x > 0);
        return isFacingRight;
    }

    private void FlipSprite()
    {
        float currentVelocitySign = Mathf.Sign(myRigidBody.velocity.x);

        transform.localScale = new Vector2(-currentVelocitySign, 1f);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Enemy" && isTimerUp)
        {
            FlipSprite();

            StartCoroutine(CollisionDetectionTimeout());
        }

        if (otherCollider.tag != "Player" && otherCollider.tag != "Enemy")
        {
            FlipSprite();
        }
    }

    private IEnumerator CollisionDetectionTimeout()
    {
        isTimerUp = false;

        yield return new WaitForSeconds(0.1f);

        isTimerUp = true;
    }
}
