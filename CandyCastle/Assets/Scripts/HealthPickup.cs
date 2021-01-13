using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
    // Config Params
    [SerializeField] AudioClip healthPickupSound = null;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] int healthValue = 1;

    // Cached References
    Player player;
    AudioManager audioManager;

    // State variables
    bool isTimerUp = true;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Player>() && isTimerUp)
        {
            Destroy(gameObject);

            audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.healthPickupSound);

            FindObjectOfType<Player>().AddHealth(healthValue);

            StartCoroutine(DelayHealthPickup());
        }
    }

    private IEnumerator DelayHealthPickup()
    {
        isTimerUp = false;

        yield return new WaitForSeconds(0.01f);

        isTimerUp = true;
    }
}
