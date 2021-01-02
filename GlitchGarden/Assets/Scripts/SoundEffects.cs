using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    // Cached References
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = PlayerPrefsController.GetEffectsVolume();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
