using UnityEngine;

public class GameSession : MonoBehaviour
{
    // State variables
    int currentScore = 0;
    //int livesLeft = 3;

    private void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        // Singleton pattern. If a GameSession object already exists, destroy this current gameStatus. If not (i.e. first time this object has loaded)
        //, put this GameSession into a new "DontDestroyOnLoad" persistent scene, that will stay when we load future scenes, each future scene then destroys their own GameSession
        // Because this one stays. This also applies to children of GameSession, i.e. gameCanvas.
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;

        if (gameStatusCount > 1)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    scoreText.text = currentScore.ToString();

    //    //livesLeftText.text = livesLeft.ToString();
    //}

    public int GetScore()
    {
        return currentScore;
    }
    
    public void AddToScore(int points)
    {
        currentScore += points;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    //public int LivesRemaining()
    //{
    //    return livesLeft;
    //}

    //public void RemoveLife()
    //{
    //    livesLeft--;
    //}
}
