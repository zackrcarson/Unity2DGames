using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void ResetGameSession()
    {
        GameObject gameSession = FindObjectOfType<GameSession>().gameObject;
        Destroy(gameSession);

        SceneManager.LoadScene(0);
    }
}
