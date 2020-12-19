using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Config Params
    [SerializeField] float loadTime = 3f;

    // State Params
    int currentSceneIndex;

    // Use this for initialization
    void Start ()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTimeAndLoadNextScene(loadTime));
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private IEnumerator WaitForTimeAndLoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadNextScene();
    }
}