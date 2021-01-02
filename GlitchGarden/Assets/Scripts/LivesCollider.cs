using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesCollider : MonoBehaviour
{
    // Config Params
    [SerializeField] AudioClip lifeLostAudio;

    // Cached references
    LivesDisplay livesDisplay;
    AudioSource audioSource;

    private void Start()
    {
        livesDisplay = FindObjectOfType<LivesDisplay>();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (lifeLostAudio)
        {
            audioSource.PlayOneShot(lifeLostAudio);
        }

        livesDisplay.LoseLife();

        Destroy(other.gameObject);
    }
}
