using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Config params
    [SerializeField] float rotationSpeedMin = -150f;
    [SerializeField] float rotationSpeedMax = 150f;
    [SerializeField] bool rotateAboutParent = false;

    float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!rotateAboutParent)
        {
            gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); 
        }
        else
        {
            gameObject.transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
        }
	}
}
