using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timeRemaning { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaning <= 0)
        {
            timerText.text = "00:00:00";
            return;
        }           

        timeRemaning -= Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(timeRemaning);
        timerText.text = time.ToString(@"hh\:mm\:ss"); //$"{Mathf.FloorToInt(timeRemaning / 3600).ToString("D2")}:{Mathf.FloorToInt(timeRemaning/60).ToString("D2")}:{Mathf.FloorToInt(timeRemaning % 60).ToString("D2")}";
    }

    public void AddTime(float value)
    {
        timeRemaning += value;
    }
}
