using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timeRemaining { get; set; }

    public bool StopTime { get; set; }

    // Update is called once per frame
    void Update()
    {
        //Used for countdown
        //if (timeRemaning <= 0)
        //{
        //    timerText.text = "00:00:00";
        //    return;
        //}
        // timeRemaining -= Time.deltaTime;
        
        timeRemaining += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = time.ToString(@"hh\:mm\:ss"); 
    }

    public string GetCurrentTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
        return time.ToString(@"hh\:mm\:ss");
    }

    //Used to add time if used as a Counter
    //public void AddTime(float value)
    //{
    //    timeRemaining += value;
    //}
}
