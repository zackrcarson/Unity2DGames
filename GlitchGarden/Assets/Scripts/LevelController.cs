using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // Config params
    [SerializeField] float levelOverDelay = 4f;
    [SerializeField] GameObject winBanner;
    [SerializeField] GameObject loseBanner;

    [SerializeField] AudioSource gameMusic;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;

    // Cached references
    AttackerSpawner[] attackerSpawners;
    LevelLoader levelLoader;
    AudioSource audioSource;
    Health health;

    // State Variables
    int numberOfEnemiesRemaining = 0;
    bool isTimerExpired = false;
    bool isLevelOver = false;

    private void Start()
    {
        health = FindObjectOfType<Health>();

        attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        levelLoader = GetComponent<LevelLoader>();

        audioSource = GetComponent<AudioSource>();

        winBanner.SetActive(false);
        loseBanner.SetActive(false);
    }

    public void AttackerSpawned()
    {
        numberOfEnemiesRemaining++;
    }

    public void AttackerDestroyed()
    {
        numberOfEnemiesRemaining--;

        CheckIfLevelWon();
    }

    private void CheckIfLevelWon()
    {
        if (isTimerExpired && numberOfEnemiesRemaining <= 0 && !isLevelOver && health.GetHealth() > 0)
        {
            StartCoroutine(HandleLevelWon());
        }
    }

    private IEnumerator HandleLevelWon()
    {
        isLevelOver = true;

        gameMusic.Stop();

        winBanner.SetActive(true);

        audioSource.PlayOneShot(winClip);

        yield return new WaitForSeconds(levelOverDelay);

        levelLoader.LoadNextLevel();
    }

    public void HandleLevelLost()
    {
        gameMusic.Stop();

        audioSource.PlayOneShot(loseClip);

        loseBanner.SetActive(true);

        Time.timeScale = 0;
    }

    private void StopSpawningAttackers()
    {
        foreach (AttackerSpawner attackerSpawner in attackerSpawners)
        {
            attackerSpawner.StopSpawning();
        }
    }

    public void TimerExpired()
    {
        isTimerExpired = true;

        StopSpawningAttackers();

        CheckIfLevelWon();
    }
}
