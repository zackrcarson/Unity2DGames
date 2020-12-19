using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderButton : MonoBehaviour
{
    // Config params
    [SerializeField] Defender defenderPrefab;

    // State variables
    bool isClicked = false;

    //Cached references
    SpriteRenderer spriteRenderer;
    DefenderButton[] defenderButtons;
    Color32 defaultColor;
    DefenderSpawner defenderSpawner;
    SunDisplay sunDisplay;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        defenderButtons = FindObjectsOfType<DefenderButton>();

        defenderSpawner = FindObjectOfType<DefenderSpawner>();

        sunDisplay = FindObjectOfType<SunDisplay>();
    }

    private void Update()
    {
        // If we don't have enough suns, make it black. If we do, and it is currently clicked, make it white. If we do, and it is NOT clicked, make it the grayed out color
        if (!sunDisplay.HaveEnoughSuns(defenderPrefab.GetSunCost()))
        {
            spriteRenderer.color = Color.black;

            isClicked = false;
        }
        else if (isClicked)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = defaultColor;
        }
    }

    public void OnMouseDown()
    {
        // Only allow us to click the button if we have enough stars for it.
        if (sunDisplay.HaveEnoughSuns(defenderPrefab.GetSunCost()))
        {
            ResetDisplay();

            spriteRenderer.color = Color.white;

            defenderSpawner.SetSelectedDefender(defenderPrefab);

            isClicked = true;
        }
    }

    private void ResetDisplay()
    {
        foreach (DefenderButton defenderButton in defenderButtons)
        {
            defenderButton.GetComponent<SpriteRenderer>().color = defaultColor;

            defenderButton.TurnOffClickedStatus();
        }
    }

    public void TurnOffClickedStatus()
    {
        isClicked = false;
    }

    public bool IsClicked()
    {
        return isClicked;
    }
}
