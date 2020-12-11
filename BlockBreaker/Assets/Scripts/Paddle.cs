using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    [SerializeField] float screenWidthUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float minY = 15f;

    GameSession gameSession;
    BlackHole ball;

    // Use this for initialization
    void Start ()
    {
        gameSession = FindObjectOfType<GameSession>();
        ball = FindObjectOfType<BlackHole>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 paddlePos = new Vector2(GetPaddleXPosition(), transform.position.y);

        paddlePos.x = Mathf.Clamp(paddlePos.x, minX, minY);

        transform.position = paddlePos;
	}

    private float GetPaddleXPosition()
    {
        // If the ball has been destroyed, we need to find it again!
        if (ball != null)
        {
            if (gameSession.IsAutoPlayEnabled() && ball.HasGameStarted())
            {
                return ball.transform.position.x;
            }
            else
            {
                return screenWidthUnits * Input.mousePosition.x / Screen.width;
            }
        }
        else
        {
            ball = FindObjectOfType<BlackHole>();

            return screenWidthUnits * Input.mousePosition.x / Screen.width;
        }
    }
}
