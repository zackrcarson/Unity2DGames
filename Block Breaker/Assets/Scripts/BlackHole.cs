using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Config parameters
    [SerializeField] Paddle paddle1;
    [SerializeField] float blackHoleRotationSpeed = 100f;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;

    [SerializeField] AudioClip[] ballSounds = null;

    // State
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached component references
    Rigidbody2D rigidBody = null;
    AudioSource myAudioSource = null;

    // Use this for initialization
    void Start ()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;

        rigidBody = GetComponent<Rigidbody2D>();

        myAudioSource = GetComponent<AudioSource>();
    }
	

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0, 0, blackHoleRotationSpeed * Time.deltaTime);

        if (!hasStarted)
        {
            LockToPaddle();
            LaunchOnClick();
        }
    }


    private void LaunchOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;

            rigidBody.velocity = new Vector2(xPush, yPush);
        }
    }


    private void LockToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);

        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
        }
    }
}
