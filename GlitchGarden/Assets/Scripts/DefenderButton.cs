using TMPro;
using UnityEngine;

public class DefenderButton : MonoBehaviour
{
    // Config params
    [SerializeField] Defender defenderPrefab;

    // State variables
    bool isClicked = false;
    int cost;

    //Cached references
    SpriteRenderer spriteRenderer;
    DefenderButton[] defenderButtons;
    Color32 defaultColor;
    DefenderSpawner defenderSpawner;
    SunDisplay sunDisplay;
    LevelController levelController;

    private void Start()
    {
        if (!defenderPrefab)
        {
            Debug.LogError(name + " has no defender prefab!");
            return;
        }
        cost = defenderPrefab.GetSunCost();

        LabelCostButton();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        defenderButtons = FindObjectsOfType<DefenderButton>();

        defenderSpawner = FindObjectOfType<DefenderSpawner>();

        sunDisplay = FindObjectOfType<SunDisplay>();

        levelController = FindObjectOfType<LevelController>();
    }

    private void LabelCostButton()
    {
        TextMeshProUGUI costText = GetComponentInChildren<TextMeshProUGUI>();

        if (!costText)
        {
            Debug.LogError(name + " has no cost!");
            return;
        }
        
        costText.text = cost.ToString();
    }

    private void Update()
    {
        // If we don't have enough suns, make it black. If we do, and it is currently clicked, make it white. If we do, and it is NOT clicked, make it the grayed out color
        if (!sunDisplay.HaveEnoughSuns(cost))
        {
            spriteRenderer.color = Color.black;

            //isClicked = false;
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
        if (!levelController.IsPaused())
        {
            // Only allow us to click the button if we have enough stars for it.
            if (sunDisplay.HaveEnoughSuns(cost))
            {
                ResetDisplay();

                spriteRenderer.color = Color.white;

                defenderSpawner.SetSelectedDefender(defenderPrefab);

                isClicked = true;
            }
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
