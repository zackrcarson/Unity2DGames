using UnityEngine;

public class Fox : MonoBehaviour
{
    // Cached references
    Attacker attacker = null;
    Animator animator = null;

    private void Start()
    {
        attacker = GetComponent<Attacker>();

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        GameObject otherObject = otherCollider.gameObject;

        if (otherObject.GetComponent<Gravestone>())
        {
            animator.SetTrigger("jumpTrigger");
        }

        else if (otherObject.GetComponent<Defender>())
        {
            attacker.Attack(otherObject);
        }
    }
}
