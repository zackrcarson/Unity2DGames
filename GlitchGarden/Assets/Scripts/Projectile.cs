using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Config parameters
    [SerializeField] float projectileSpeed = 6f;
    [SerializeField] float projectileRotation = -500f;
    [SerializeField] int projectileDamage = 10;

    // Update is called once per frame
    void Update ()
    {
        transform.Translate(projectileSpeed * Time.deltaTime, 0, 0, Space.World);
        transform.Rotate(0, 0, projectileRotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var otherHealth = otherCollider.GetComponent<Health>();
        var otherAttacker = otherCollider.GetComponent<Attacker>();
            
        if (otherHealth && otherAttacker)
        {
            otherHealth.DealDamage(projectileDamage);

            Destroy(gameObject);
        }
    }
}
