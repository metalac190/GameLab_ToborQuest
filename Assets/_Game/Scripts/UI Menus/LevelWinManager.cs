using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelWinManager : MonoBehaviour
{
    public HUDManager hudManager;
    public TextMeshProUGUI winText;
    public GameObject returnLevelSelectButton;
    public GameObject _children;
    public string LevelSaveTimeName = "Level1BestTime";

    private void OnEnable()
    {
        CGSC.OnWinGame += WinGamePanel;
    }

    private void OnDisable()
    {
        CGSC.OnWinGame -= WinGamePanel;
    }

    private void Start()
    {
        _children.SetActive(false);
    }

    private void WinGamePanel()
    {        
        _children.SetActive(true);
        winText.text = "You arrived in " + hudManager.GetCurrentTimeText();
        hudManager.currentTimerText.StopTime = true;
        EventSystem.current.SetSelectedGameObject(returnLevelSelectButton);
        //PlayerPrefs.Save(LevelSaveTimeName, hudManager.currentTimerText.timeRemaining);
    }

}
