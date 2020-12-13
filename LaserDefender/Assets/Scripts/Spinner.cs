using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Config params
    [SerializeField] float rotationSpeedMin = -150f;
    [SerializeField] float rotationSpeedMax = 150f;

    float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    // Update is called once per frame
    void Update ()
    {
        gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); 
	}
}
