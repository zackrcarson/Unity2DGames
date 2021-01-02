using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    // Constant string reference keys
    const string MUSIC_VOLUME_KEY = "music volume";
    const string EFFECTS_VOLUME_KEY = "effects volume";
    const string DIFFICULTY_KEY = "difficulty";

    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;

    const int MIN_DIFFICULTY = 1;
    const int MAX_DIFFICULTY = 5;

    public static void SetMusicVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Error: Music Volume " + volume + " is out of range (" + MIN_VOLUME + ", " + MAX_VOLUME + ").");
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static void SetEffectsVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(EFFECTS_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Error: Effects Volume " + volume + " is out of range (" + MIN_VOLUME + ", " + MAX_VOLUME + ").");
        }
    }

    public static float GetEffectsVolume()
    {
        return PlayerPrefs.GetFloat(EFFECTS_VOLUME_KEY);
    }

    public static void SetDifficulty(int difficulty)
    {
        if (difficulty >= MIN_DIFFICULTY && difficulty <= MAX_DIFFICULTY)
        {
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
        }
        else
        {
            Debug.LogError("Error: Difficulty " + difficulty + " is out of range (" + MIN_DIFFICULTY + ", " + MAX_DIFFICULTY + ").");
        }
    }

    public static int GetDifficulty()
    {
        return (int)PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }
}