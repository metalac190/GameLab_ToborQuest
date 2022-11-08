using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class HUDManager : PersistableObject
{
    public GameObject pausePanel;

    public MovementInput _controller;

    public TimerUI currentTimerText;
    public Slider toborProgress;
    public TextMeshProUGUI bestTime;
    private MovementController _movementCtrl;
    [SerializeField]
    private Image _boostImage;
    [SerializeField]
    private GameObject pauseFirstButton;

    private LevelWinManager levelWinManager;

    private float toborProgressValue;
    private float timeElapsed;

    private void Awake()
    {        
        _controller = new MovementInput();
        _movementCtrl = FindObjectOfType<MovementController>(true);
        levelWinManager = FindObjectOfType<LevelWinManager>(true);
        pausePanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(levelWinManager.LevelSaveName))
        {
            float bestTimeFloat = PlayerPrefs.GetFloat(levelWinManager.LevelSaveName);
            SetBestTime(bestTimeFloat);
        }
        else
        {
            SetBestTime(180f);
        }
        SetToborProgress(0f);        
    }

    private void OnEnable()
    {        
        timeElapsed = 0;
        toborProgressValue = 0;
        currentTimerText.timeRemaining = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        if (CGSC.GameOver)
            return;

        SetBoostPercentage();
        //Tobor progress bar
        //if (timeElapsed < 10f)
        //{
        //    toborProgressValue = Mathf.Lerp(0, 1, timeElapsed / 10f);
        //    timeElapsed += Time.deltaTime;
        //}
        //SetToborProgress(toborProgressValue);

    }

    public void SetBestTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        bestTime.text = time.ToString(@"mm\:ss\:fff");
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

    private void SetBoostPercentage()
    {        
        _boostImage.fillAmount = _movementCtrl.BoostPercentage();
    }

    public void StartTimer()
    {
        currentTimerText.StartTimer();
    }

}
