using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    // Config params
    [SerializeField] int healthBonus = 100;
    
    public int GetHealthBoost()
    {
        return healthBonus;
    }

    public void PickedUp()
    {
        Destroy(gameObject);
    }
}
