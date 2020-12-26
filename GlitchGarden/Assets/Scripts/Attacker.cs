using UnityEngine;

public class Attacker : MonoBehaviour
{
    //           |   Lizard     Fox     Jello     Jabba
    // --------------------------------------------------
    //  Speed    |   Medium     Fast    Medium     Slow
    //  Attack   |   Medium     Low      None      Huge
    //  Health   |   Medium     Low     Medium     Huge
    //  Special  |   None       Jump   Ghosting      ?

    // Cached references
    Animator animator = null;
    LevelController levelController = null;

    // State parameters
    float currentSpeed = 1f;
    GameObject currentTarget = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        levelController = FindObjectOfType<LevelController>();
        levelController.AttackerSpawned();
    }

    private void OnDestroy()
    {
        levelController.AttackerDestroyed();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (!currentTarget)
        {
            StopAttacking();
        }
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void Attack(GameObject target)
    {
        animator.SetBool("isAttacking", true);

        currentTarget = target;
    }

    public void StopAttacking()
    {
        animator.SetBool("isAttacking", false);
    }

    public void StrikeCurrentTarget(int damage)
    {
        if (!currentTarget) { return; }

        Health health = currentTarget.GetComponent<Health>();

        if (!health) { return; }

        health.DealDamage(damage);
    }
}
