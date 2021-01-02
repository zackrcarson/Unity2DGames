using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    // Config parameters
    [Tooltip("Level timer in seconds.")]
    [SerializeField] float baseLevelTime = 10f;
    [SerializeField] float difficultyModifier = 1f;
    float levelTime;
    [SerializeField] AudioClip timerUpClip;
    float initialLevelDelay = 4f;

    // Cached References
    Slider slider;
    Animator animator;
    LevelController levelController;
    AudioSource timerAudioSource;

    // State variables
    bool timerExpired = false;
    int difficulty;
    float timeSinceStart;

    private void Start()
    {
        difficulty = PlayerPrefsController.GetDifficulty();

        levelTime = baseLevelTime + difficultyModifier * baseLevelTime * (((float)difficulty - 3) / 5);

        timerAudioSource = GetComponent<AudioSource>();

        animator = GetComponentInChildren<Animator>();

        slider = GetComponent<Slider>();

        levelController = FindObjectOfType<LevelController>();

        StartCoroutine(TimerDelay());
    }

    // Update is called once per frame
    void Update ()
    {
        if (timerExpired) { return; }

        timeSinceStart = Time.timeSinceLevelLoad - initialLevelDelay;

        slider.value = timeSinceStart / levelTime;

        timerExpired = (timeSinceStart >= levelTime);

        if (timerExpired)
        {
            animator.enabled = false;

            timerAudioSource.PlayOneShot(timerUpClip);

            levelController.TimerExpired();
        }
    }

    private IEnumerator TimerDelay()
    {
        animator.enabled = false;

        yield return new WaitForSeconds(initialLevelDelay);

        animator.enabled = true;
    }
}
