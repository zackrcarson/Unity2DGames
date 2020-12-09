using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameSession gameStatus;

    private void Start()
    {
        gameStatus = FindObjectOfType<GameSession>();
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadNextScene(float delay)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        StartCoroutine(LoadNextSceneAfterDelay(delay, currentSceneIndex));
    }

    public void LoadFirstScene()
    {
        gameStatus.ResetGame();

        int startSceneIndex = 0;

        SceneManager.LoadScene(startSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadNextSceneAfterDelay(float delay, int currentSceneIndex)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
