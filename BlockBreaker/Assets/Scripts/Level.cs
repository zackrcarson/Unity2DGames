using UnityEngine;

public class Level : MonoBehaviour
{
    // Parameters
    [SerializeField] int breakableBlocks; // Serialized for debugging purpose
    [SerializeField] float loadTime = 0.75f;
    // Cached reference
    SceneLoader sceneLoader = null;
    BlackHole gameBall = null;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        gameBall = FindObjectOfType<BlackHole>();
    }

    public void CountBlocks()
    {
        breakableBlocks++;
    }

    public void BlockDestroyed()
    {
        breakableBlocks--;

        if (breakableBlocks <= 0)
        {
            gameBall.DestroyBall();
            sceneLoader.LoadNextScene(loadTime);
        }
    }
}
