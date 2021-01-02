using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // Config Params
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject gunTransform;

    // Cached References
    AttackerSpawner myLaneSpawner;
    Animator animator;
    
    GameObject projectileParent;
    const string PROJECTILE_PARENT_NAME = "Projectiles";

    private void Start()
    {
        animator = GetComponent<Animator>();

        SetLaneSpawner();

        CreateProjectileParent();
    }

    public void CreateProjectileParent()
    {
        projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);

        if (!projectileParent)
        {
            projectileParent = new GameObject(PROJECTILE_PARENT_NAME);
        }
    }

    private void Update()
    {
        if (!myLaneSpawner) { return; }

        if (IsAttackerInLane() && IsAttackerInFront())
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private bool IsAttackerInLane()
    {
        if (myLaneSpawner.transform.childCount <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsAttackerInFront()
    {
        Attacker[] attackers = myLaneSpawner.GetComponentsInChildren<Attacker>();

        foreach (Attacker attacker in attackers)
        {
            bool isInFront = attacker.transform.position.x - transform.position.x >= 0f;
            
            if (isInFront)
            {
                return true;
            }
        }

        return false;
    }

    private void SetLaneSpawner()
    {
        AttackerSpawner[] attackSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner attackerSpawner in attackSpawners)
        {
            bool isCloseEnough = Mathf.Abs(attackerSpawner.transform.position.y - transform.position.y) <= Mathf.Epsilon;

            if (isCloseEnough)
            {
                myLaneSpawner = attackerSpawner;
            }
        }
    }

    public void Fire()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, gunTransform.transform.position, Quaternion.identity) as GameObject;

        newProjectile.transform.parent = projectileParent.transform;
    }
}
