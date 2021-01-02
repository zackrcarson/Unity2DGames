using UnityEngine;
using System.Collections.Generic;

public class DefenderSpawner : MonoBehaviour
{
    // Config Params
    [SerializeField] AudioClip spawnAudio;

    // Cached References
    Defender defender = null;
    SunDisplay sunDisplay;
    DefenderButton[] defenderButtons;
    LevelController levelController;

    GameObject defenderParent;
    const string DEFENDER_PARENT_NAME = "Defenders";

    AudioSource audioSource;

    // State Variables
    Dictionary<string, int> defenderArrayDict;

    private void Start()
    {
        CreateDefenderParent();

        audioSource = GetComponent<AudioSource>();

        sunDisplay = FindObjectOfType<SunDisplay>();

        defenderButtons = FindObjectsOfType<DefenderButton>();

        defenderArrayDict = new Dictionary<string, int>();

        levelController = FindObjectOfType<LevelController>();
    }

    public void CreateDefenderParent()
    {
        defenderParent = GameObject.Find(DEFENDER_PARENT_NAME);

        if (!defenderParent)
        {
            defenderParent = new GameObject(DEFENDER_PARENT_NAME);
        }
    }

    public void SetSelectedDefender(Defender defenderToSet)
    {
        defender = defenderToSet;
    }

    void OnMouseDown()
    {
        if (!levelController.IsPaused())
        {
            AttemptToSpawnDefender(GetSquareClicked());
        }
    }

    private void AttemptToSpawnDefender(Vector2 gridPos)
    {
        string locationKey = ((int)gridPos.x).ToString() + ":" + ((int)gridPos.y).ToString();

        bool keyExists = defenderArrayDict.ContainsKey(locationKey);

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
        if (!defender || !areAnyClicked || keyExists) { return; }

        int spawnCost = defender.GetSunCost();

        // Spawn the defender if we have enough suns!
        if (sunDisplay.HaveEnoughSuns(spawnCost))
        {
            SpawnDefender(gridPos);

            sunDisplay.RemoveSuns(spawnCost);

            defenderArrayDict.Add(locationKey, 1);
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
        if (spawnAudio)
        {
            audioSource.PlayOneShot(spawnAudio);
        }

        Defender newDefender = Instantiate(defender, spawnCoordinates, Quaternion.identity) as Defender;

        newDefender.transform.parent = defenderParent.transform;
    }

    public void RemoveKey(Vector2 worldPos)
    {
        Vector2 gridPos = SnapToGrid(worldPos);

        string keyToRemove = ((int)gridPos.x).ToString() + ":" + ((int)gridPos.y).ToString();

        defenderArrayDict.Remove(keyToRemove);
    }
}
