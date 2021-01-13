using UnityEngine;

public class PlayerChooser : MonoBehaviour
{
    // Config Params
    [SerializeField] int activePlayerNumber = 0;

    // Cached References
    Player[] players;

    private void Start()
    {
        players = FindObjectsOfType<Player>();
    }

    public int GetActivePlayerNumber()
    {
        return activePlayerNumber;
    }

    public void SetActivePlayerNumber(int activePlayerNumberIn)
    {
        activePlayerNumber = activePlayerNumberIn;

        foreach (Player player in players)
        {
            player.IsActivePlayer(activePlayerNumber);
        }
    }
}
