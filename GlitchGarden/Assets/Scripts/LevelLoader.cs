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
        finalSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex == 0 || currentSceneIndex == 1)
        {
            StartCoroutine(WaitForTimeAndLoadStartScene(loadTime));
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
            SceneManager.LoadScene("Win Screen"); // TODO: Make a win screen?
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

    public void LoadSplashScreenNoDefaultPrefs()
    {
        ResetTimeScale();

        SceneManager.LoadScene("Splash Screen No Default Prefs");
    }

    public void LoadStartScreen()
    {
        ResetTimeScale();

        SceneManager.LoadScene("Start Screen");
    }

    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene("Options Screen");
    }

    private IEnumerator WaitForTimeAndLoadStartScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadStartScreen();
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