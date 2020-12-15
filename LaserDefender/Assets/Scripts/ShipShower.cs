using UnityEngine;
using UnityEngine.UI;

public class ShipShower : MonoBehaviour
{
    // Config params
    [SerializeField] Sprite[] shipArray = null;

    // Cached References
    Image image = null;
    GameSession gameSession = null;

	// Use this for initialization
	void Start ()
    {
        image = GetComponent<Image>();
        gameSession = FindObjectOfType<GameSession>();
    }
	
	public void ChangeShip(int arrayNumber)
    {
        image.sprite = shipArray[arrayNumber - 1];

        gameSession.SetPlayerShipNumber(arrayNumber);
    }
}
