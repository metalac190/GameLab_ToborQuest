using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timeRemaining { get; set; }

    public bool startTimer = false;

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
        if (!startTimer)
            return;

        timeRemaining += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
        if(timeRemaining > 3600)
            timerText.text = time.ToString(@"hh\:mm\:ss\:fff");
        else
            timerText.text = time.ToString(@"mm\:ss\:fff"); 
    }

    public string GetCurrentTime()
    {
        string timeFormat;
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
        if (timeRemaining > 3600)
            timeFormat = time.ToString(@"hh\:mm\:ss\:fff");
        else
            timeFormat = time.ToString(@"mm\:ss\:fff");
        return timeFormat;
    }

    public void StartTimer()
    {
        startTimer = true;
    }

    //Used to add time if used as a Counter
    //public void AddTime(float value)
    //{
    //    timeRemaining += value;
    //}
}
