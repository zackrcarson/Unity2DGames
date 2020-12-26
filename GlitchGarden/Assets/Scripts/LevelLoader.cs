using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Config Params
    [SerializeField] float loadTime = 3f;

    // State Params
    int currentSceneIndex;

    // Cached references
    int finalSceneIndex;

    // Use this for initialization
    void Start ()
    {
        finalSceneIndex = SceneManager.sceneCount - 1;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTimeAndLoadNextScene(loadTime));
        }
    }

    public void ReloadLevel()
    {
        ResetTimeScale();

        SceneManager.LoadScene(currentSceneIndex);
    }

    private static void ResetTimeScale()
    {
        Time.timeScale = 1;
    }

    public void LoadNextLevel()
    {
        ResetTimeScale();

        if (currentSceneIndex == finalSceneIndex)
        {
            SceneManager.LoadScene("Win Screen");
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void LoadFirstLevel()
    {
        ResetTimeScale();

        SceneManager.LoadScene("Level 1");
    }

    public void LoadSplashScreen()
    {
        ResetTimeScale();

        SceneManager.LoadScene("Splash Screen");
    }

    private IEnumerator WaitForTimeAndLoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadNextLevel();
    }

    public void LoadGameOver()
    {
        ResetTimeScale();

        SceneManager.LoadScene("Game Over Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}