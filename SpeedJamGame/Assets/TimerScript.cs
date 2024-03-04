using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TMP_Text timerText;
    private float startTime;
    private bool timerActive;
    public float t;
    void Start()
    {
        timerActive = false;
    }

    void Update()
    {
        if (!timerActive && Input.anyKeyDown)
        {
            StartTimerWithInput();
        }

        if (timerActive)
        {
            t = Time.time - startTime;

            string minutes = Mathf.Floor(t / 60).ToString("00");
            string seconds = Mathf.Floor(t % 60).ToString("00");
            string milliseconds = Mathf.Floor((t * 1000) % 1000).ToString("000");

            timerText.text = minutes + ":" + seconds + ":" + milliseconds;
        }
    }

    public void StartTimerWithInput()
    {
        if (!timerActive)
        {
            startTime = Time.time;
            timerActive = true;
        }
    }

    public void StopTimer()
    {
        timerActive = false;
    }
}
