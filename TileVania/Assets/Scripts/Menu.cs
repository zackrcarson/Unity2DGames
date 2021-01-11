﻿using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    // Config Params
    [SerializeField] float openDoorDelay = 1f;
    [SerializeField] float playerDropOffset = 1f;

    // Cached References
    LevelExit levelExit;
    Player player;

    private void Start()
    {
        levelExit = FindObjectOfType<LevelExit>();
        player = FindObjectOfType<Player>();

        levelExit.SetFirstLastLevel();
    }

    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenDoor()
    {
        if (levelExit)
        {
            player.PauseControls();
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            player.transform.position = levelExit.transform.position + new Vector3(0f, playerDropOffset, 0f);

            StartCoroutine(TimeDelayAndOpenDoor());
        }
        else
        {
            StartFirstLevel();
        }
    }

    IEnumerator TimeDelayAndOpenDoor()
    {
        yield return new WaitForSeconds(openDoorDelay);

        levelExit.ExternallyOpenDoor();
    }
}
