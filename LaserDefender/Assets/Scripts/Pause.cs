using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    // Config Variables
    [SerializeField] AudioSource musicPlayer = null;
    [SerializeField] GameObject pauseMenu = null;

    // State variables
    bool isPaused = false;

    private void Awake()
    {
        pauseMenu.SetActive(false);

        isPaused = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0;
        musicPlayer.Pause();

        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1;
        musicPlayer.Play();

        pauseMenu.SetActive(false);
    }

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void SwitchPauseStatus()
    {
        isPaused = !isPaused;
    }
}
