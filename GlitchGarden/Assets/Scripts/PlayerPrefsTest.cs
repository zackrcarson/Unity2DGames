using UnityEngine;

public class PlayerPrefsTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        PlayerPrefsController.SetMusicVolume(0.4f);

        Debug.Log("The saved volume was found to be " + PlayerPrefsController.GetMusicVolume());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
