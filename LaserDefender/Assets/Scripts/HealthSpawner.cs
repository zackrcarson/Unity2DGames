using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    // Config Params
    [Header("Spawn Settings")]
    [SerializeField] float dropCounter = 8f;
    [SerializeField] float minTimeBetweenDrops = 0.2f;
    [SerializeField] float maxTimeBetweenDrops = 3f;
    //[SerializeField] float dropSpeed = 30f;
    [SerializeField] GameObject[] healthPrefabs;

    [SerializeField] float xBoundaryPadding = 0.5f;
    [SerializeField] float yBoundaryPadding = 0.5f;

    // State params
    float xMin, xMax, yMin, yMax;

    // Use this for initialization
    void Start()
    {
        SetupMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndDrop();
    }

    private void CountDownAndDrop()
    {
        dropCounter -= Time.deltaTime;

        if (dropCounter <= 0f)
        {
            Drop();
        }
    }

    private void Drop()
    {
        float xPos = Random.Range(xMin, xMax);
        float yPos = Random.Range(yMin, yMax);
        Vector2 newPos = new Vector2(xPos, yPos);

        int prefabNumber = WeightedProbability();

        //GameObject pill = Instantiate(healthPrefabs[prefabNumber], newPos, Quaternion.identity) as GameObject;
        Instantiate(healthPrefabs[prefabNumber], newPos, Quaternion.identity);

        //pill.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

        dropCounter = Random.Range(minTimeBetweenDrops, maxTimeBetweenDrops);
    }


    private int WeightedProbability()
    {
        float rand = Random.value;
        if (rand <= 0.4f)
        {
            return 0;
        }
        else if (rand > 0.4f && rand <= 0.7f)
        {
            return 1;
        }
        else if (rand > 0.7f && rand <= 0.9f)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }


    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xBoundaryPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xBoundaryPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yBoundaryPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yBoundaryPadding;
    }
}
