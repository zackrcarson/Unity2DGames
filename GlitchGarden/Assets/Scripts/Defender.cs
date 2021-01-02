using UnityEngine;

public class Defender : MonoBehaviour
{
    //         |   Trophy     Cactus     Gnome     Grave     |    Lizard      Fox
    // --------|---------------------------------------------|----------------------
    //  Cost   |     100         50        200       100     |      /          /           
    //  Health |     40         50        50        250      |     500        300          
    //  Damage |     /          50        200        /       |      20         40       

    // Level Progression:
    // cactus + lizard
    // + trophies
    // + gravestones 
    // + foxes
    // + gnomes

    //           |   Lizard     Fox     Jello     Jabba
    // --------------------------------------------------
    //  Speed    |   Medium     Fast    Medium     Slow
    //  Attack   |   Medium     Low      None      Huge
    //  Health   |   Medium     Low     Medium     Huge
    //  Special  |   None       Jump   Ghosting      ?

    // Config params
    [SerializeField] int sunCost = 100;
    [SerializeField] int sunsPerCycle = 5;

    // Cached references
    SunDisplay sunDisplay;

    private void Start()
    {
        sunDisplay = FindObjectOfType<SunDisplay>();
    }

    public void AddSuns()
    {
        sunDisplay.AddSuns(sunsPerCycle);
    }

    public int GetSunCost()
    {
        return sunCost;
    }
}
