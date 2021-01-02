using UnityEngine;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    // Config params
    [SerializeField] int baseLives = 5;
    int lives;

    [SerializeField] int livesPerHit = 1;

    // Cached references
    TextMeshProUGUI livesText;
    LevelController levelController;

    // Use this for initialization
    void Start()
    {
        int difficulty = PlayerPrefsController.GetDifficulty();

        lives = baseLives - difficulty + 1;

        if (lives < 1) { lives = 1; }

        levelController = FindObjectOfType<LevelController>();
        livesText = GetComponent<TextMeshProUGUI>();

        UpdateLivesDisplay();
    }

    private void UpdateLivesDisplay()
    {
        livesText.text = lives.ToString();
    }

    public void LoseLife()
    {
        lives -= livesPerHit;

        UpdateLivesDisplay();

        if (lives <= 0)
        {
            lives = 0;
            UpdateLivesDisplay();

            levelController.HandleLevelLost();
        }
    }

    public void GainLife()
    {
        lives += livesPerHit;

        UpdateLivesDisplay();
    }

    public int GetLives()
    {
        return lives;
    }
}
