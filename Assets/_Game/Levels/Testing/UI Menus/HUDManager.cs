using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : PersistableObject
{
    public GameObject pausePanel;

    public TimerUI currentTimerText;
    public Slider toborProgress;
    public TextMeshProUGUI bestTime;

    private float toborProgressValue;
    private float timeElapsed;
    // Start is called before the first frame update
    void Awake()
    {
        SetBestTime(180f);
        SetToborProgress(0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Update tobor progress once you have a variable to track.
        //SetToborProgress();
        //Example of what it could look like
        if(timeElapsed < 10f)
        {
            toborProgressValue = Mathf.Lerp(0, 1, timeElapsed / 10f);
            timeElapsed += Time.deltaTime;
        }        
        SetToborProgress(toborProgressValue);

        if (!Input.GetKeyDown(KeyCode.P))
            return;

        pausePanel.SetActive(!pausePanel.activeInHierarchy);
    }

    public void SetBestTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        bestTime.text = time.ToString(@"hh\:mm\:ss");
    }

    public void SetToborProgress(float value)
    {
        toborProgress.value = value;
    }

}
