using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Config Params
    [SerializeField] int health = 100;
    [SerializeField] GameObject deathVFX;

    [SerializeField] GameObject hitVFX;

    [SerializeField] float VFXLifetime = 1f;


    public void DealDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            TriggerHitVFX();
        }
    }

    private void TriggerHitVFX()
    {
        if (!hitVFX) { return; }

        GameObject hit = Instantiate(hitVFX, gameObject.transform.position, Quaternion.identity) as GameObject;

        Destroy(hit, VFXLifetime);
    }

    private void Die()
    {
        TriggerDeathVFX();

        Destroy(gameObject);
    }

    private void TriggerDeathVFX()
    {
        if (!deathVFX) { return; }

        GameObject death = Instantiate(deathVFX, gameObject.transform.position, Quaternion.identity) as GameObject;

        Destroy(death, VFXLifetime);
    }
}
