﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // Config parameters
    WaveConfig waveConfig;

    List<Transform> waypoints;
    float moveSpeed;

    int waypointIndex = 0;

	// Use this for initialization
	void Start ()
    {
        waypoints = waveConfig.GetWaypoints();

        moveSpeed = waveConfig.GetEnemyMoveSpeed();

        transform.position = waypoints[waypointIndex].transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
