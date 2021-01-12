using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    // Config parameters
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] Sprite doorOpenSprite = null;

    // Cached References
    SpriteRenderer myRenderer;
    Player player;
    GameSession gameSession;

    // State variables
    bool isOnDoor = false;
    bool isFirstLastLevel = false;
    bool isGameSessionFound = false;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            isOnDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            isOnDoor = false;
        }
    }

    private void Update()
    {
        if (!isGameSessionFound)
        {
            gameSession = FindObjectOfType<GameSession>();

            isGameSessionFound = true;
        }

        if (!isFirstLastLevel)
        {
            if (isOnDoor && Input.GetAxisRaw("Vertical") > 0)
            {
                StartCoroutine(OpenDoorDelayAndLoadNextScene());
            }
        }
    }

    IEnumerator OpenDoorDelayAndLoadNextScene()
    {
        gameSession.ResetCoinsFromThisLevel();

        gameSession.UpdateUI();

        myRenderer.sprite = doorOpenSprite;

        player.PauseControls();
        player.SetDoorWalkingAnimation(levelLoadDelay);
        
        yield return new WaitForSeconds(levelLoadDelay);

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        gameSession.ResetInitialUIUpdated();

        var totalNumberOfScenes = SceneManager.sceneCountInBuildSettings;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == totalNumberOfScenes - 1)
        {
            gameSession.SelfDestruct();

            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void ExternallyOpenDoor()
    {
        StartCoroutine(OpenDoorDelayAndLoadNextScene());
    }

    public void SetFirstLastLevel()
    {
        isFirstLastLevel = true;
    }
}
