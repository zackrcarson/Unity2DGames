using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberWizard : MonoBehaviour {

    [SerializeField] public int min;
    [SerializeField] public int max;

    [SerializeField] TextMeshProUGUI GuessText;

    int guess;

    List<int> previousGuesses = new List<int>();

    // Use this for initialization
    void Start ()
    {
        StartGame();
    }
	

    void StartGame()
    {
        NextGuess();

        GuessText.text = guess.ToString();
    }

    public void OnPressHigher()
    {
        if (!(min >= max))
        {
            min = guess + 1;

            NextGuess();

            GuessText.text = guess.ToString();
        }
    }

    public void OnPressLower()
    {
        if (!(min >= max))
        {
            max = guess;

            NextGuess();

            GuessText.text = guess.ToString();
        }
    }

    void NextGuess()
    {

        if (min >= max)
        {
            min = max;
            guess = min;
        }
        else
        {
            if (previousGuesses.Count == 0)
            {
                guess = Random.Range(min, max + 1);
            }

            while (previousGuesses.Contains(guess))
            {
                guess = Random.Range(min, max + 1);
            }
        }

        previousGuesses.Add(guess);
    }
}
