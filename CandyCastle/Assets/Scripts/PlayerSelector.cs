using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    // State variables
    bool isGameSessionFound = false;
    int currentSceneIndex = 0;

    // Cached references
    GameSession gameSession;
    Player[] players;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameSessionFound && currentSceneIndex != 0)
        {
            gameSession = FindObjectOfType<GameSession>();
            isGameSessionFound = true;

            int activePlayerNumber = gameSession.GetActivePlayerNumber();

            players = FindObjectsOfType<Player>();

            foreach (Player player in players)
            {
                if (!player.AmIActive())
                {
                    Destroy(player.gameObject);
                }
            }
        }
    }
}
