using UnityEngine;

public class Lizard : MonoBehaviour
{
    // Cached references
    Attacker attacker = null;

    private void Start()
    {
        attacker = GetComponent<Attacker>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        GameObject otherObject = otherCollider.gameObject;

        if (otherObject.GetComponent<Defender>())
        {
            attacker.Attack(otherObject);
        }
    }
}
