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

    // State variables
    bool isOnDoor = false;
    bool isFirstLastLevel = false;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
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
        myRenderer.sprite = doorOpenSprite;

        player.PauseControls();
        player.SetDoorWalkingAnimation(levelLoadDelay);
        
        yield return new WaitForSeconds(levelLoadDelay);

        LoadNextScene();
    }

    private static void LoadNextScene()
    {
        var totalNumberOfScenes = SceneManager.sceneCountInBuildSettings;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == totalNumberOfScenes - 1)
        {
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
