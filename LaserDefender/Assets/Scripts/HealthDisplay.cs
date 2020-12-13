using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    // Cached References
    Player player = null;
    TextMeshProUGUI healthText = null;

    private void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = Mathf.Clamp(player.GetHealth(), 0, 999999).ToString();
    }
}
