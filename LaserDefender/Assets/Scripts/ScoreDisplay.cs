using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    // Cached References
    GameSession gameSession = null;
    TextMeshProUGUI scoreText = null;

    // State Variable
    bool isSessionSetup = false;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSessionSetup)
        {
            gameSession = FindObjectOfType<GameSession>();

            isSessionSetup = true;
        }

        scoreText.text = gameSession.GetScore().ToString();
    }
}
