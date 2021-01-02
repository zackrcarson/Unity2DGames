using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    // Config Variables
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider difficultySlider;

    [SerializeField] float defaultMusicVolume = 0.1f;
    [SerializeField] float defaultEffectsVolume = 1f;
    [SerializeField] int defaultDifficulty = 3;

    // Cached References
    MusicPlayer musicPlayer;
    PauseMusicPlayer pauseMusicPlayer;
    LevelLoader levelLoader;

	// Use this for initialization
	void Start ()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        pauseMusicPlayer = FindObjectOfType<PauseMusicPlayer>();

        levelLoader = FindObjectOfType<LevelLoader>();

        musicSlider.value = PlayerPrefsController.GetMusicVolume();
        effectsSlider.value = PlayerPrefsController.GetEffectsVolume();
        difficultySlider.value = PlayerPrefsController.GetDifficulty();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (musicPlayer)
        {
            musicPlayer.SetVolume(musicSlider.value);
        }
        else
        {
            Debug.LogWarning("No Music Player Found!");
        }

        if (pauseMusicPlayer)
        {
            pauseMusicPlayer.SetVolume(musicSlider.value);
        }
    }

    public void SetDefaults()
    {
        SetAudioDefaults();

        difficultySlider.value = defaultDifficulty;
    }

    public void SetAudioDefaults()
    {
        musicSlider.value = defaultMusicVolume;
        effectsSlider.value = defaultEffectsVolume;
    }

    public void SaveAndExit()
    {
        SaveAndDontExit();

        levelLoader.LoadStartScreen();
    }

    public void SaveAndDontExit()
    {
        PlayerPrefsController.SetMusicVolume(musicSlider.value);
        PlayerPrefsController.SetEffectsVolume(effectsSlider.value);
        PlayerPrefsController.SetDifficulty((int)difficultySlider.value);
    }
}
