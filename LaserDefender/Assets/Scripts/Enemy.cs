using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Config parameters
    [Header("Enemy")]
    [SerializeField] float health = 500;
    [SerializeField] int pointsPerDeath = 150;

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

    // Use this for initialization
    void Start ()
    {
        mainCamera = Camera.main;

        gameSession = FindObjectOfType<GameSession>();

        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }
	
	// Update is called once per frame
	void Update ()
    {
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

        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        DamageDealer damageDealer = otherCollider.gameObject.GetComponent<DamageDealer>();

        if (damageDealer == null) { return; }

        if (otherCollider.tag != "Enemy" && otherCollider.tag != "Player")
        {
            damageDealer.Hit();
        }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();

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
    }
}
