using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timeRemaining { get; set; }
    public static float levelTime;

    public bool startTimer = false;

    private void Start()
    {
        if (CGSC.PlayingQuest) timeRemaining = levelTime;
        else timeRemaining = 0;
        UpdateTimerText();
    }

    private void Update()
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
        levelTime = timeRemaining;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        float totalTimeRemaining = timeRemaining;
        if (CGSC.PlayingQuest)
        {
            totalTimeRemaining += CGSC.TotalTime;
        }
        timerText.text = ConvertTimeToText(totalTimeRemaining);
    }

    public static string ConvertTimeToText(float t)
	{
		if (t == 0) return "--:--:--";
        TimeSpan time = TimeSpan.FromSeconds(t);
        return time.ToString(t > 3600 ? @"hh\:mm\:ss\:ff" : @"mm\:ss\:ff");
    }

	public string GetCurrentTime() => ConvertTimeToText(timeRemaining);

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
