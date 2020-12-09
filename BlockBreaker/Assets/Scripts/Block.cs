using UnityEngine;

public class Block : MonoBehaviour
{
    // config params
    [SerializeField] float blockDestroyDelay = 0f;
    [SerializeField] float particleDestroyDelay = 1f;

    [SerializeField] AudioClip breakClip = null;
    [SerializeField] GameObject blockBreakingVFX = null;
    [SerializeField] Sprite[] hitSprites = null;

    // Cached references
    Level level = null;
    GameSession gameSession = null;
    SpriteRenderer spriteRenderer = null;

    // State variables
    [SerializeField] int timesHit = 0; // Serialized for debug purposes

    private void Start()
    {
        level = FindObjectOfType<Level>();
        gameSession = FindObjectOfType<GameSession>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;

        int maxHits = hitSprites.Length + 1;

        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;

        if (hitSprites[spriteIndex] != null)
        {
            spriteRenderer.sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block Sprite is missing from array: " + gameObject.name);
        }
    }

    private void DestroyBlock()
    {

        gameSession.AddToScore();

        AudioSource.PlayClipAtPoint(breakClip, Camera.main.transform.position);

        Destroy(gameObject, blockDestroyDelay);

        level.BlockDestroyed();

        TriggerBreakingVFX();
    }

    private void TriggerBreakingVFX()
    {
        GameObject sparkles = Instantiate(blockBreakingVFX, transform.position, transform.rotation);
        Destroy(sparkles, particleDestroyDelay);
    }
}
