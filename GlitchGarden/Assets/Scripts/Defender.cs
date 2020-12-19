using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    // Config params
    [SerializeField] int sunCost = 100;

    // Cached references
    SunDisplay sunDisplay;

    private void Start()
    {
        sunDisplay = FindObjectOfType<SunDisplay>();
    }

    public void AddSuns(int amount)
    {
        sunDisplay.AddSuns(amount);
    }

    public int GetSunCost()
    {
        return sunCost;
    }
}
