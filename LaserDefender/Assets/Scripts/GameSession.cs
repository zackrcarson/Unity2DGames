﻿using UnityEngine;

public class GameSession : MonoBehaviour
{
    // State variables
    int currentScore = 0;
    [SerializeField] int playerShipNumber = 1;

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

    public void SetPlayerShipNumber(int shipNumber)
    {
        playerShipNumber = shipNumber;
    }

    public int GetPlayerShipNumber()
    {
        return playerShipNumber;
    }
}