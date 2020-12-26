using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesCollider : MonoBehaviour
{
    // Cached references
    LivesDisplay livesDisplay;

    private void Start()
    {
        livesDisplay = FindObjectOfType<LivesDisplay>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        livesDisplay.LoseLife();

        Destroy(other.gameObject);
    }
}
