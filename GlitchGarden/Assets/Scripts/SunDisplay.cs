using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SunDisplay : MonoBehaviour
{
    // Config Params
    [SerializeField] int suns = 100;
    int maxSuns = 9999;

    // Cached references
    TextMeshProUGUI sunText;

    // Use this for initialization
    void Start()
    {
        sunText = GetComponent<TextMeshProUGUI>();

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        sunText.text = suns.ToString();
    }

    public void AddSuns(int amount)
    {
        if (suns + amount <= maxSuns)
        {
            suns += amount;
        }
        else
        {
            suns = maxSuns;
        }
        
        UpdateDisplay();
    }

    public void RemoveSuns(int amount)
    {
        if (suns >= amount)
        {
            suns -= amount;

            UpdateDisplay();
        }
    }

    public bool HaveEnoughSuns(int amount)
    {
        return (suns >= amount);
    }
}
