using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour {

    int ballsLost = 0;
    int totalBalls;

    GameSession gameSession = null;
    SceneLoader sceneLoader = null;

    private void Start()
    {
        totalBalls = GameObject.FindGameObjectsWithTag("ball").Length;

        gameSession = FindObjectOfType<GameSession>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ballsLost++;

        if (ballsLost >= totalBalls && gameSession.LivesRemaining() > 0)
        {
            gameSession.RemoveLife();

            sceneLoader.ReloadCurrentScene();
        }
        else if (ballsLost >= totalBalls && gameSession.LivesRemaining() <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
