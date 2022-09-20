using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class HUDManager : PersistableObject
{
    public GameObject pausePanel;

    public MovementInput _controller;

    public TimerUI currentTimerText;
    public Slider toborProgress;
    public TextMeshProUGUI bestTime;

    private float toborProgressValue;
    private float timeElapsed;

    private void Awake()
    {
        _controller = new MovementInput();
        pausePanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBestTime(180f);
        SetToborProgress(0f);        
    }

    private void OnEnable()
    {
        _controller.Enable();
    }

    private void OnDisable()
    {
        _controller.Disable();
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

        if (_controller.UI.Pause.IsPressed())
        {
            Debug.Log("Game Paused");
            PauseGame();
            //Debug.Log("Game Unpaused");
            //return;
        }        
        
    }

    public void SetBestTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        bestTime.text = time.ToString(@"hh\:mm\:ss");
    }

    public string GetBestTimeString()
    {
        return bestTime.text;
    }

    public string GetCurrentTimeText()
    {
        return currentTimerText.timerText.text;
    }

    public void SetToborProgress(float value)
    {
        toborProgress.value = value;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        //gameObject.SetActive(false);
        Time.timeScale = 0;
    }

}
