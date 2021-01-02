using UnityEngine;

public class Jelly : MonoBehaviour
{
    // Cached references
    Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        GameObject otherObject = otherCollider.gameObject;

        if (otherObject.GetComponent<Defender>())
        {
            animator.SetTrigger("phaseTrigger");
        }
    }
}
