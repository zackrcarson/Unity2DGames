using UnityEngine;

public class Attacker : MonoBehaviour
{
    // Config Params
    [SerializeField] float difficultyModifier = 1f;
    [SerializeField] int damage = 30;
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpSpeed = 1.5f;

    // Cached references
    Animator animator = null;
    LevelController levelController = null;

    // State parameters
    float currentSpeed = 1f;
    GameObject currentTarget = null;
    int difficulty;

    private void Awake()
    {
        difficulty = PlayerPrefsController.GetDifficulty();

        animator = GetComponent<Animator>();

        levelController = FindObjectOfType<LevelController>();
        levelController.AttackerSpawned();
    }

    private void OnDestroy()
    {
        if (levelController != null)
        {
            levelController.AttackerDestroyed();
        }
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

    public void SetJumpMovementSpeed()
    {
        float modifiedSpeed = jumpSpeed + difficultyModifier * jumpSpeed * (((float)difficulty - 3) / 5);

        currentSpeed = modifiedSpeed;
    }

    public void SetCustomMovementSpeed(float inputSpeed)
    {
        float modifiedSpeed = inputSpeed + difficultyModifier * inputSpeed * (((float)difficulty - 3) / 5);

        currentSpeed = modifiedSpeed;
    }

    public void SetMovementSpeed()
    {
        float modifiedSpeed = speed + difficultyModifier * speed * (((float)difficulty - 3) / 5);

        currentSpeed = modifiedSpeed;
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

    public void StrikeCurrentTarget()
    {
        int modifiedDamage = Mathf.RoundToInt(damage + difficultyModifier * damage * (((float)difficulty - 3) / 5));

        if (!currentTarget) { return; }

        Health health = currentTarget.GetComponent<Health>();

        if (!health) { return; }

        health.DealDamage(modifiedDamage);
    }
}
