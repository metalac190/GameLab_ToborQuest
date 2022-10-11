using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelWinManager : MonoBehaviour
{
    private HUDManager hudManager;
    private TextMeshProUGUI winText;
    private GameObject returnLevelSelectButton;
    private GameObject _children;
    [SerializeField]
    private string levelSaveTimeName = "Level1BestTime";

    public string LevelSaveName { get { return levelSaveTimeName; } }

    private void Awake()
    {
        //Find hudmanager, should only be 1 in the scene.
        hudManager = GameObject.FindObjectOfType<HUDManager>();

        //Get needed objects
        _children = this.transform.GetChild(0).gameObject;
        winText = _children.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        returnLevelSelectButton = _children.transform.GetChild(1).gameObject;
    }

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
        SaveBestTime();
    }

    private void SaveBestTime()
    {
        float currentTime = hudManager.currentTimerText.timeRemaining;
        if (PlayerPrefs.HasKey(levelSaveTimeName))
        {
            float previousBestTime = PlayerPrefs.GetFloat(levelSaveTimeName);            
            if (previousBestTime > currentTime)
            {
                //Debug.Log("New best time");
                PlayerPrefs.SetFloat(levelSaveTimeName, currentTime);
            }
            else
            {
                //Debug.Log("Didn't beat best time");
            }
        }
        else
        {
            //Debug.Log("Best time set");
            PlayerPrefs.SetFloat(levelSaveTimeName, currentTime);
        }
            
    }

}
