using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // Config Params
    [SerializeField] int playerLives = 3;
    [SerializeField] float deathDelay = 3f;

    [SerializeField] Text healthText = null;
    [SerializeField] Text livesText = null;
    [SerializeField] Text coinsText = null;

    [SerializeField] Image playerLivesSpriteRenderer = null;
    [SerializeField] Sprite[] playerLivesSprites = null;

    // Cached References
    Player player;
    UI ui;

    // State Variables
    int numberCoins = 0;
    int coinsThisLevel = 0;

    int activePlayerNumber = 0;

    bool UISetUp = false;
    bool initialUIUpdated = false;

    bool newPlayerSetup = false;

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

    private void Start()
    {
        FindPlayer();

        UpdateUI();
    }

    private void Update()
    {
        if (!UISetUp)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                ui = Resources.FindObjectsOfTypeAll<UI>()[0];
                ui.gameObject.SetActive(false);
            }
            else
            {
                ui = Resources.FindObjectsOfTypeAll<UI>()[0];
                ui.gameObject.SetActive(true);

                if (ui.enabled)
                {
                    UISetUp = true;
                }
                else
                {
                    ui.gameObject.SetActive(true);
                }
            }
        }

        if (!initialUIUpdated)
        {
            UpdateUI();

            initialUIUpdated = true;
        }
    }

    public void UpdateUI()
    {
        if (!player) { FindPlayer(); }

        if (!newPlayerSetup) { FindPlayer(); newPlayerSetup = true; }

        if (healthText)
        {
            healthText.text = player.GetHealth().ToString();
        }
        
        livesText.text = playerLives.ToString();
        coinsText.text = numberCoins.ToString();
    }

    public void ProcessPlayerDeath()
    {
        playerLives--;

        initialUIUpdated = true;
        UpdateUI();

        StartCoroutine(DeathDelayAndReload());
    }

    private IEnumerator DeathDelayAndReload()
    {
        UpdateUI();

        yield return new WaitForSeconds(deathDelay);

        numberCoins -= coinsThisLevel;
        ResetCoinsFromThisLevel();

        UpdateUI();

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

        initialUIUpdated = false;
    }

    public int GetLives()
    {
        return playerLives;
    }

    public int GetCoins()
    {
        return numberCoins;
    }

    public void AddCoins(int number)
    {
        numberCoins += number;
        coinsThisLevel += number;

        UpdateUI();
    }

    public void ResetCoinsFromThisLevel()
    {
        coinsThisLevel = 0;
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void ResetInitialUIUpdated()
    {
        initialUIUpdated = false;
    }

    public void SetActivePlayerNumber(int activePlayerNumberIn)
    {
        activePlayerNumber = activePlayerNumberIn;

        playerLivesSpriteRenderer.sprite = playerLivesSprites[activePlayerNumberIn];
    }

    public int GetActivePlayerNumber()
    {
        return activePlayerNumber;
    }
}
