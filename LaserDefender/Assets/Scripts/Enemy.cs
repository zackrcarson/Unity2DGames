using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Config parameters
    [Header("Enemy")]
    [SerializeField] float health = 500;
    [SerializeField] int pointsPerDeath = 150;
    [SerializeField] RectTransform healthBar;

    [Header("Projectile")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 30f;

    [Header("Prefabs")]
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] GameObject explosionPrefab = null;
    [SerializeField] float explosionLifeTime = 1f;

    [Header("Audio")]
    [SerializeField] AudioClip laserAudio = null;
    [SerializeField] AudioClip deathAudio = null;
    [SerializeField] [Range(0, 1)] float laserAudioVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float deathAudioVolume = 0.7f;

    // Cached references
    Camera mainCamera = null;
    GameSession gameSession = null;
    float maximumHealth;
    float healthBarSize;

    // State variables
    bool isScoreSetup = false;

    // Use this for initialization
    void Start ()
    {
        maximumHealth = health;
        healthBarSize = healthBar.sizeDelta.x;

        mainCamera = Camera.main;

        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isScoreSetup)
        {
            gameSession = FindObjectOfType<GameSession>();

            isScoreSetup = true;
        }

        CountDownAndShoot();
	}

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        AudioSource.PlayClipAtPoint(laserAudio, mainCamera.transform.position, laserAudioVolume);

        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

        if (tag == "Boss")
        {
            GameObject laser1 = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

            laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 3, -projectileSpeed);

            GameObject laser2 = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 3, -projectileSpeed);
        }

        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        DamageDealer damageDealer = otherCollider.gameObject.GetComponent<DamageDealer>();

        if (damageDealer == null) { return; }

        if (otherCollider.tag != "Enemy" && otherCollider.tag != "Player" && otherCollider.tag != "Boss")
        {
            damageDealer.Hit();
        }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();

        healthBar.sizeDelta = new Vector2((health / maximumHealth) * healthBarSize, healthBar.sizeDelta.y);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameSession.AddToScore(pointsPerDeath);

        Destroy(gameObject);

        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, explosionLifeTime);

        AudioSource.PlayClipAtPoint(deathAudio, mainCamera.transform.position, deathAudioVolume);

        if (tag == "Boss")
        {
            FindObjectOfType<EnemySpawner>().SetBossDead();
        }
    }
}
