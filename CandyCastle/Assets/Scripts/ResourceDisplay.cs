using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    // Config Params
    [SerializeField] Text healthText = null;
    [SerializeField] Text livesText = null;
    [SerializeField] Text coinsText = null;

    // Cached References
    Player player;
    GameSession gameSession;
    
    // State Variables
    int playerLives = 0;
    int playerHealth = 0;
    int playerCoins = 0;

    bool isGameSessionFound = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        gameSession = FindObjectOfType<GameSession>();

        playerHealth = player.GetHealth();
        healthText.text = playerHealth.ToString();

        playerLives = gameSession.GetLives();
        livesText.text = playerLives.ToString();

        playerCoins = gameSession.GetCoins();
        coinsText.text = playerCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameSessionFound)
        {
            gameSession = FindObjectOfType<GameSession>();

            isGameSessionFound = true;
        }

        healthText.text = player.GetHealth().ToString();

        livesText.text = gameSession.GetLives().ToString();

        coinsText.text = gameSession.GetCoins().ToString();
    }
}
