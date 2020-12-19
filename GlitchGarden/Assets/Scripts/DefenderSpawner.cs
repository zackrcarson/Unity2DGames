using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    // Config Params
    Defender defender = null;

    // Cached References
    SunDisplay sunDisplay;
    DefenderButton[] defenderButtons;

    private void Start()
    {
        sunDisplay = FindObjectOfType<SunDisplay>();

        defenderButtons = FindObjectsOfType<DefenderButton>();
    }

    public void SetSelectedDefender(Defender defenderToSet)
    {
        defender = defenderToSet;
    }

    void OnMouseDown()
    {
        AttemptToSpawnDefender(GetSquareClicked());
    }

    private void AttemptToSpawnDefender(Vector2 gridPos)
    {
        // Check if we actually have any buttons clicked (Sometimes the defender has been populated, but the button was un-clicked)
        bool areAnyClicked = false;
        foreach (DefenderButton defenderButton in defenderButtons)
        {
            if (defenderButton.IsClicked())
            {
                areAnyClicked = true;
                break;
            }
        }

        // Don't spawn anything if either the defender isn't populated yet, or if so, still don't do it if no buttons are pressed
        if (!defender || !areAnyClicked) { return; }

        int spawnCost = defender.GetSunCost();

        // Spawn the defender if we have enough suns!
        if (sunDisplay.HaveEnoughSuns(spawnCost))
        {
            SpawnDefender(gridPos);

            sunDisplay.RemoveSuns(spawnCost);
        }
    }

    private Vector2 GetSquareClicked()
    {
        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);

        Vector2 gridPos = SnapToGrid(worldPos);
        return gridPos;
    }

    private Vector2 SnapToGrid(Vector2 rawWorldPosition)
    {
        float newX = Mathf.RoundToInt(rawWorldPosition.x);
        float newY = Mathf.RoundToInt(rawWorldPosition.y);

        return new Vector2(newX, newY);
    }

    private void SpawnDefender(Vector2 spawnCoordinates)
    {
        Defender newDefender = Instantiate(defender, spawnCoordinates, Quaternion.identity) as Defender;

        newDefender.transform.parent = transform;
    }
}
