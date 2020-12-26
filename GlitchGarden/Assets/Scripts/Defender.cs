using UnityEngine;

public class Defender : MonoBehaviour
{
    //         |   Trophy     Cactus     Gnome     Grave
    // --------------------------------------------------
    //  Cost   |   Medium      Low       High      Medium
    //  Damage |   None        Low       High      None
    //  Health |   Low         Low       Low       High


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
