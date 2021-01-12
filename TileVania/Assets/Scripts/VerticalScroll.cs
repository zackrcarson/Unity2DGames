using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    // Config Params
    [SerializeField] float scrollRate = 0.5f;
    [SerializeField] float scrollDelay = 2f;

    // State variables
    bool isTimerUp = false;

    private void Start()
    {
        StartCoroutine(SetTimer());
    }

    private IEnumerator SetTimer()
    {
        yield return new WaitForSeconds(scrollDelay);

        isTimerUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerUp)
        {
            Vector2 yMovement = new Vector2(0f, scrollRate * Time.deltaTime);
            transform.Translate(yMovement);
        }
    }
}
