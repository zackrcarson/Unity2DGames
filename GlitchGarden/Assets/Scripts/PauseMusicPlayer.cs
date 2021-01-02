using UnityEngine;

public class PauseMusicPlayer : MonoBehaviour
{
    // Cached References
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(this);

        audioSource = GetComponent<AudioSource>();

        audioSource.volume = PlayerPrefsController.GetMusicVolume();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
