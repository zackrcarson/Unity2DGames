using UnityEngine;

public class PlayerPrefsRestart : MonoBehaviour
{
    [SerializeField] float defaultMusicVolume = 0.1f;
    [SerializeField] float defaultEffectsVolume = 1f;
    [SerializeField] int defaultDifficulty = 3;

    // Use this for initialization
    void Start ()
    {
        PlayerPrefsController.SetMusicVolume(defaultMusicVolume);
        PlayerPrefsController.SetEffectsVolume(defaultEffectsVolume);
        PlayerPrefsController.SetDifficulty(defaultDifficulty);
    }
}
