using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] Text storyText;
    
    [SerializeField] State startingState;
    
    State state;

    // Use this for initialization
    void Start ()
    {
        state = startingState;
        storyText.text = state.GetStateStory();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ManageState();
	}

    private void ManageState()
    {
        State[] nextStates = state.GetNextStates();

        for (int index = 0; index < nextStates.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                state = nextStates[index];
            }
        }

        storyText.text = state.GetStateStory();
    }
}