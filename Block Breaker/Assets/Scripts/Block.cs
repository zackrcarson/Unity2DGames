using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config
    [SerializeField] float destroyDelay = 0f;
    [SerializeField] AudioClip breakClip = null;

    // Cached references
    Level level;
    GameSession gameSession;

    private void Start()
    {
        level = FindObjectOfType<Level>();
        level.CountBreakableBlocks();

        gameSession = FindObjectOfType<GameSession>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBlock();
    }

    private void DestroyBlock()
    {
        gameSession.AddToScore();

        AudioSource.PlayClipAtPoint(breakClip, Camera.main.transform.position);
        Destroy(gameObject, destroyDelay);

        level.BlockDestroyed();
    }
}
