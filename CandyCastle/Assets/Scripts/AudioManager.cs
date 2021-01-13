using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Config Params
    [SerializeField] AudioClip mainMenuMusic = null;
    [SerializeField] AudioClip bossMusic = null;
    [SerializeField] AudioClip successMusic = null;
    [SerializeField] AudioClip[] levelMusicArray = null;
    [SerializeField] AudioClip[] deathMusicArray = null;

    [SerializeField] float musicVolume = 0.5f;

    // Cached References
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = musicVolume;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int numberOfScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex == 0)
        {
            audioSource.clip = mainMenuMusic;
        }
        else if (currentSceneIndex == numberOfScenes - 2)
        {
            audioSource.clip = bossMusic;
        }
        else if (currentSceneIndex == numberOfScenes - 1)
        {
            audioSource.clip = successMusic;
        }
        else
        {
            int randomClipNumber = Random.Range(0, levelMusicArray.Length);

            audioSource.clip = levelMusicArray[randomClipNumber];
        }

        audioSource.Play();
    }

    public AudioSource ClipPlayAtPoint(Vector3 pos, float spatial_blend, AudioClip clip)
    {
        GameObject go = new GameObject("TmpAudio");
        go.transform.position = pos;
        AudioSource audio_source = go.AddComponent<AudioSource>();
        audio_source.spatialBlend = spatial_blend;
        audio_source.clip = clip;
        audio_source.Play();
        Destroy(go, clip.length);
        return audio_source;
    }

    public void PlayGameOverMusic()
    {
        audioSource.Stop();

        StartCoroutine(PlayTwoClipsInARow());
    }

    private IEnumerator PlayTwoClipsInARow()
    {
        audioSource.loop = false;

        audioSource.clip = deathMusicArray[0];
        audioSource.Play();

        yield return new WaitForSeconds(deathMusicArray[0].length);

        audioSource.Stop();

        audioSource.loop = true;

        audioSource.clip = deathMusicArray[1];
        audioSource.Play();
    }
}
