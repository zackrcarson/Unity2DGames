using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Config Params
    [SerializeField] int health = 100;
    [SerializeField] GameObject deathVFX;

    [SerializeField] GameObject hitVFX;

    [SerializeField] float VFXLifetime = 1f;

    [SerializeField] AudioClip hitAudio;
    [SerializeField] AudioClip deathAudio;

    [SerializeField] GameObject damageTextAnimationPrefab;
    [SerializeField] float TextAnimationLifeTime = 1f;
    [SerializeField] float TextAnimationOffset = 1f;
    
    // State Variables
    bool isDefender;

    // Cached References
    DefenderSpawner defenderSpawner = null;
    AudioSource audioSource;
    SpriteRenderer[] spriteRenderers;

    GameObject VFXParent;
    const string VFX_PARENT_NAME = "VFX";

    private void Start()
    {
        CreateVFXParent();

        audioSource = GetComponent<AudioSource>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        defenderSpawner = FindObjectOfType<DefenderSpawner>();

        Defender defender = GetComponent<Defender>();

        if (defender) { isDefender = true; }
        else { isDefender = false;  }
    }

    private void CreateVFXParent()
    {
        VFXParent = GameObject.Find(VFX_PARENT_NAME);

        if (!VFXParent)
        {
            VFXParent = new GameObject(VFX_PARENT_NAME);
        }
    }

    public void DealDamage(int damage)
    {
        DamageTextAnimation(damage);

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

    private void DamageTextAnimation(int damage)
    {
        Vector3 textOffset = new Vector3(0f, TextAnimationOffset, 0f);

        GameObject damageTextAnimation = Instantiate(damageTextAnimationPrefab, transform.position + textOffset, transform.rotation);
        damageTextAnimation.GetComponentInChildren<TextMeshProUGUI>().text = "-" + damage.ToString();

        Destroy(damageTextAnimation, TextAnimationLifeTime);
    }

    private void TriggerHitVFX()
    {
        if (hitAudio)
        {
            audioSource.PlayOneShot(hitAudio);
        }

        if (hitVFX)
        {
            GameObject newHitVFX = Instantiate(hitVFX, gameObject.transform.position, Quaternion.identity) as GameObject;

            newHitVFX.transform.parent = VFXParent.transform;

            Destroy(newHitVFX, VFXLifetime);
        }
    }

    private void Die()
    {
        TriggerDeathVFX();

        if (isDefender)
        {
            defenderSpawner.RemoveKey(gameObject.transform.position);
        }

        if (deathAudio)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = false;
            }
            Destroy(gameObject, deathAudio.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void TriggerDeathVFX()
    {
        if (deathAudio)
        {
            audioSource.PlayOneShot(deathAudio);
        }

        if (deathVFX)
        {
            GameObject newDeathVFX = Instantiate(deathVFX, gameObject.transform.position, Quaternion.identity) as GameObject;

            newDeathVFX.transform.parent = VFXParent.transform;

            Destroy(newDeathVFX, VFXLifetime);
        }
    }
}
