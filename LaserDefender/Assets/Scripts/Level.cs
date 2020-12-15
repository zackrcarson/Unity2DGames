using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // Config parameters
    [SerializeField] float gameOverDelayTime = 2f;

    public void LoadGameOver()
    {
        StartCoroutine(DelayedLoadGameOver());
    }

    IEnumerator DelayedLoadGameOver()
    {
        yield return new WaitForSeconds(gameOverDelayTime);

        SceneManager.LoadScene("Game Over");
    }

    public void LoadPlayerSelect()
    {
        SceneManager.LoadScene("Player Select");
    }

    public void LoadPlayerSelectWithReset()
    {
        SceneManager.LoadScene("Player Select");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameWithoutReset()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void LoadStartMenuWithReset()
    {
        SceneManager.LoadScene("Start Menu");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
