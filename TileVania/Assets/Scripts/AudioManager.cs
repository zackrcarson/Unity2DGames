using UnityEngine;

public class AudioManager : MonoBehaviour
{
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
}
