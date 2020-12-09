using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    [SerializeField] float screenWidthUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float minY = 15f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float mousePosInUnits = screenWidthUnits * Input.mousePosition.x / Screen.width;
        mousePosInUnits = Mathf.Clamp(mousePosInUnits, minX, minY);

        Vector2 paddlePos = new Vector2(mousePosInUnits, transform.position.y);
        transform.position = paddlePos;
	}
}
