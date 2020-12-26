using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    // Config params
    [SerializeField] int lives = 5;
    [SerializeField] int livesPerHit = 1;

    // Cached references
    TextMeshProUGUI livesText;
    LevelController levelController;

    // Use this for initialization
    void Start()
    {
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
}
