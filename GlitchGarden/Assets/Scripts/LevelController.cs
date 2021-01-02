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

    [SerializeField] GameObject pauseMenu;

    // Cached references
    AttackerSpawner[] attackerSpawners;
    LevelLoader levelLoader;
    AudioSource audioSource;
    LivesDisplay lives;
    OptionsController optionsController;
    SoundEffects[] soundEffectAudioSources;

    // State Variables
    int numberOfEnemiesRemaining = 0;
    bool isTimerExpired = false;
    bool isLevelOver = false;
    bool isPaused = false;

    private void Start()
    {
        lives = FindObjectOfType<LivesDisplay>();

        attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        levelLoader = GetComponent<LevelLoader>();

        audioSource = GetComponent<AudioSource>();

        soundEffectAudioSources = FindObjectsOfType<SoundEffects>();

        winBanner.SetActive(false);
        loseBanner.SetActive(false);

        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {
        gameMusic.Stop();

        Time.timeScale = 0;

        pauseMenu.SetActive(true);

        isPaused = true;
    }

    public void UnpauseGame()
    {
        gameMusic.Play();

        optionsController = FindObjectOfType<OptionsController>();
        optionsController.SaveAndDontExit();

        foreach (SoundEffects soundEffect in soundEffectAudioSources)
        {
            soundEffect.SetVolume(PlayerPrefsController.GetEffectsVolume());
        }

        Time.timeScale = 1;

        pauseMenu.SetActive(false);

        isPaused = false;
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
        if (isTimerExpired && numberOfEnemiesRemaining <= 0 && !isLevelOver && lives.GetLives() > 0)
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

    public bool IsPaused()
    {
        return isPaused;
    }
}
