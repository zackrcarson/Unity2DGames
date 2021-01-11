using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // Config Params
    [SerializeField] int playerLives = 3;
    [SerializeField] float deathDelay = 3f;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        playerLives--;

        StartCoroutine(DeathDelayAndReload());
    }

    private IEnumerator DeathDelayAndReload()
    {
        yield return new WaitForSeconds(deathDelay);

        if (playerLives <= 0)
        {
            GameOverScreen gameOverScreen = Resources.FindObjectsOfTypeAll<GameOverScreen>()[0];

            gameOverScreen.SetActive();
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    public int GetLives()
    {
        return playerLives;
    }
}
