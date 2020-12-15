using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Config Parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float xBoundaryPadding = 0.5f;
    [SerializeField] float yBoundaryPadding = 0.5f;
    [SerializeField] int health = 1000;

    [Header("VFX")]
    [SerializeField] GameObject explosionPrefab = null;
    [SerializeField] GameObject healthVFXPrefab = null;
    [SerializeField] GameObject healthTextAnimationPrefab = null;
    [SerializeField] GameObject damageTextAnimationPrefab = null;
    [SerializeField] float explosionLifeTime = 1f;
    [SerializeField] float healthVFXLifeTime = 1f;
    [SerializeField] float TextAnimationLifeTime = 1f;
    [SerializeField] float TextAnimationOffset = 1f;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 30f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] GameObject laserPrefab = null;

    [Header("Audio")]
    [SerializeField] AudioClip laserAudio = null;
    [SerializeField] AudioClip deathAudio1 = null;
    [SerializeField] AudioClip deathAudio2 = null;
    [SerializeField] AudioClip hitAudio = null;
    [SerializeField] AudioClip healthAudio = null;
    [SerializeField] [Range(0, 1)] float laserAudioVolume = 0.1f;
    [SerializeField] [Range(0, 1)] float deathAudio1Volume = 1f;
    [SerializeField] [Range(0, 1)] float deathAudio2Volume = 1f;
    [SerializeField] [Range(0, 1)] float hitAudioVolume = 1f;
    [SerializeField] [Range(0, 1)] float healthAudioVolume = 1f;

    [Header("Player Ship Sprites")]
    [SerializeField] Sprite[] shipArray = null;
    [SerializeField] GameObject[] shipColliderArray = null;

    float xMin, xMax, yMin, yMax;

    // State variables
    bool isShipSetup = false;

    // Cached references
    Coroutine firingCoroutine = null;
    Camera mainCamera = null;
    Level level = null;
    Pause pauseMenu = null;
    GameSession gameSession = null;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;

        level = FindObjectOfType<Level>();

        pauseMenu = FindObjectOfType<Pause>();

        SetupMoveBoundaries();
	}

    // Update is called once per frame
    void Update()
    {
        if (!isShipSetup)
        {
            SetupShip();
        }

        if (!pauseMenu.IsPaused())
        {
            Move();

            Fire();
        }
	}

    private void SetupShip()
    {
        gameSession = FindObjectOfType<GameSession>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        int shipNumber = gameSession.GetPlayerShipNumber() - 1;

        PolygonCollider2D playerCollider = GetComponent<PolygonCollider2D>();

        if (shipNumber % 3 == 1)
        {
            playerCollider.points = shipColliderArray[0].GetComponent<PolygonCollider2D>().points;
        }
        else if (shipNumber % 3 == 2)
        {
            playerCollider.points = shipColliderArray[1].GetComponent<PolygonCollider2D>().points;
        }
        else
        {
            playerCollider.points = playerCollider.points;
        }

        spriteRenderer.sprite = shipArray[shipNumber];

        isShipSetup = true;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            AudioSource.PlayClipAtPoint(laserAudio, mainCamera.transform.position, laserAudioVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        DamageDealer damageDealer = otherCollider.gameObject.GetComponent<DamageDealer>();
        HealthBoost healthBoost = otherCollider.gameObject.GetComponent<HealthBoost>();

        if (damageDealer == null && healthBoost == null) { return; }
        
        if (damageDealer != null)
        {
            if (otherCollider.tag != "Enemy" && otherCollider.tag != "Player" && otherCollider.tag != "Boss")
            {
                damageDealer.Hit();
            }

            ProcessHit(damageDealer);
        }

        if (healthBoost != null)
        {
            AddHealth(healthBoost.GetHealthBoost());

            healthBoost.PickedUp();
        }

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int damage = damageDealer.GetDamage();

        health -= damage;

        Vector3 textOffset = new Vector3(0f, TextAnimationOffset, 0f);

        GameObject damageTextAnimation = Instantiate(damageTextAnimationPrefab, transform.position + textOffset, transform.rotation);
        damageTextAnimation.GetComponentInChildren<TextMeshPro>().text = "-" + damage.ToString();

        Destroy(damageTextAnimation, TextAnimationLifeTime);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioSource.PlayClipAtPoint(hitAudio, mainCamera.transform.position, hitAudioVolume);
        }
    }

    private void Die()
    {
        level.LoadGameOver();

        Destroy(gameObject);

        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, explosionLifeTime);

        AudioSource.PlayClipAtPoint(deathAudio1, mainCamera.transform.position, deathAudio1Volume);
        AudioSource.PlayClipAtPoint(deathAudio2, mainCamera.transform.position, deathAudio2Volume);
    }

    public int GetHealth()
    {
        return health;
    }

    private void AddHealth(int healthBonus)
    {
        health += healthBonus;
        AudioSource.PlayClipAtPoint(healthAudio, mainCamera.transform.position, healthAudioVolume);

        GameObject healthVFX = Instantiate(healthVFXPrefab, transform.position, transform.rotation);
        Destroy(healthVFX, healthVFXLifeTime);

        Vector3 textOffset = new Vector3(0f, TextAnimationOffset, 0f);

        GameObject healthTextAnimation = Instantiate(healthTextAnimationPrefab, transform.position + textOffset, transform.rotation);
        healthTextAnimation.GetComponentInChildren<TextMeshPro>().text = "+" + healthBonus.ToString();

        Destroy(healthTextAnimation, TextAnimationLifeTime);
    }

    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xBoundaryPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xBoundaryPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yBoundaryPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yBoundaryPadding;
    }
}
