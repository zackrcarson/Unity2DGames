using UnityEngine;

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
        if (otherCollider.GetComponent<Player>())
        {
            audioManager.ClipPlayAtPoint(Camera.main.transform.position, 0.0f, this.coinPickupSound);

            if (!isGameSessionFound)
            {
                gameSession = FindObjectOfType<GameSession>();

                isGameSessionFound = true;
            }

            gameSession.AddCoins(coinValue);

            Destroy(gameObject);
        }
    }
}
