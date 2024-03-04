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
    
    void Start()
    {
        timerActive = false;
    }

    
    void Update()
    {

        if(!timerActive && Input.anyKey)
        {
            StartTimerWithInput();
        }

        if (timerActive)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;
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
