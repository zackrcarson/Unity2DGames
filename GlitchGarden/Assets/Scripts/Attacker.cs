using UnityEngine;

public class Attacker : MonoBehaviour
{
    // State parameters
    float currentSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }
}
