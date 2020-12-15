using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // Parameters
    [Range(0.1f, 10f)] [SerializeField] float gameSpeed = 1f;
    [SerializeField] int pointsPerBlockDestroyed = 83;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI autoPlayText = null;
    [SerializeField] TextMeshProUGUI livesLeftText = null;

    [SerializeField] bool autoPlay = false;

    // State variables
    int currentScore = 0;
    int livesLeft = 3;

    private void Awake()
    {
        // Singleton pattern. If a GameSession object already exists, destroy this current gameStatus. If not (i.e. first time this object has loaded)
        //, put this GameSession into a new "DontDestroyOnLoad" persistent scene, that will stay when we load future scenes, each future scene then destroys their own GameSession
        // Because this one stays. This also applies to children of GameSession, i.e. gameCanvas.
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        scoreText.text = currentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = gameSpeed;

        scoreText.text = "Score: " + currentScore.ToString();

        if (Input.GetKeyDown(KeyCode.A))
        {
            autoPlay = !autoPlay;
        }

        if (autoPlay)
        {
            autoPlayText.text = "Auto-Play ON (A key)\nSpeed = " + gameSpeed.ToString() + " (1-4 keys)";
        }
        else
        {
            autoPlayText.text = "Auto-Play OFF (A key)\nSpeed = " + gameSpeed.ToString() + " (1-4 keys)";
        }

        livesLeftText.text = livesLeft.ToString();


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameSpeed = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameSpeed = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameSpeed = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameSpeed = 4;
        }
    }

    public void AddToScore()
    {
        currentScore += pointsPerBlockDestroyed;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public bool IsAutoPlayEnabled()
    {
        return autoPlay;
    }

    public int LivesRemaining()
    {
        return livesLeft;
    }

    public void RemoveLife()
    {
        livesLeft--;
    }
}
