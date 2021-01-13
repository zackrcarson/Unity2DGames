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
    //bool isFirstLastLevel = false;
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

        //if (!isFirstLastLevel)
        //{
        if (isOnDoor && Input.GetAxisRaw("Vertical") > 0)
        {

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                foreach (Player tempPlayer in FindObjectsOfType<Player>())
                {
                    if (tempPlayer.AmIActive())
                    {
                        player = tempPlayer;
                        break;
                    }
                }

                int activePlayerNumber = FindObjectOfType<PlayerChooser>().GetActivePlayerNumber();
                FindObjectOfType<GameSession>().SetActivePlayerNumber(activePlayerNumber);
            }
            else
            {
                if (!player)
                {
                    player = FindObjectOfType<Player>();
                }
            }

            player.SetExitingRoom();

            StartCoroutine(OpenDoorDelayAndLoadNextScene());
        }
        //}
    }

    IEnumerator OpenDoorDelayAndLoadNextScene()
    {
        gameSession.ResetCoinsFromThisLevel();

        gameSession.UpdateUI();

        myRenderer.sprite = doorOpenSprite;

        foreach (Player tempPlayer in FindObjectsOfType<Player>())
        {
            if (tempPlayer.AmIActive())
            {
                player = tempPlayer;
                break;
            }
        }

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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            foreach (Player tempPlayer in FindObjectsOfType<Player>())
            {
                if (tempPlayer.AmIActive())
                {
                    player = tempPlayer;
                    break;
                }
            }

            int activePlayerNumber = FindObjectOfType<PlayerChooser>().GetActivePlayerNumber();
            FindObjectOfType<GameSession>().SetActivePlayerNumber(activePlayerNumber);
        }

        StartCoroutine(OpenDoorDelayAndLoadNextScene());
    }

    //public void SetFirstLastLevel()
    //{
    //    isFirstLastLevel = true;
    //}
}
