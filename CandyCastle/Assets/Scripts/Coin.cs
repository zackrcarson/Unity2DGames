using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    // Config Params
    [SerializeField] AudioClip coinPickupSound = null;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] int coinValue = 5;

    // Cached References
    GameSession gameSession;
    AudioManager audioManager;

    // State Variables
    bool isGameSessionFound = false;
    bool isTimerUp = true;

    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        audioManager = FindObjectOfType<AudioManager>();
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

            audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.coinPickupSound);

            if (!isGameSessionFound)
            {
                gameSession = FindObjectOfType<GameSession>();

                isGameSessionFound = true;
            }

            gameSession.AddCoins(coinValue);

            StartCoroutine(DelayCoinPickup());
        }
    }

    private IEnumerator DelayCoinPickup()
    {
        isTimerUp = false;

        yield return new WaitForSeconds(0.01f);

        isTimerUp = true;
    }
}
