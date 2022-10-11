using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelWinManager : MonoBehaviour
{
    private HUDManager hudManager;
    [SerializeField]
    private TextMeshProUGUI winText;
    [SerializeField]
    private GameObject returnLevelSelectButton;
    private GameObject _children;
    [SerializeField]
    private string levelSaveTimeName = "Level1BestTime";
    [SerializeField]
    private int nextLevel = 0; 
    public string LevelSaveName { get { return levelSaveTimeName; } }

    private void Awake()
    {
        //Find hudmanager, should only be 1 in the scene.
        hudManager = GameObject.FindObjectOfType<HUDManager>();

        //Get needed objects
        //_children = this.transform.GetChild(0).gameObject;
        //winText = _children.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //returnLevelSelectButton = _children.transform.GetChild(1).gameObject;
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
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //this.transform.GetChild();
    }

    private void WinGamePanel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        winText.text = "DELIVERY TIME: " + hudManager.GetCurrentTimeText();
        hudManager.currentTimerText.StopTime = true;
        EventSystem.current.SetSelectedGameObject(returnLevelSelectButton);
        SaveBestTime();
    }

    public void ContinueNextLevel()
    {
        CGSC.UnpauseGame();
        CGSC.LoadScene(CGSC.QuestNames[nextLevel + 1]);
    }

    public void RestartLevel()
    {
        CGSC.RestartLevel();
    }

    public void ReturnToLevels()
    {
        CGSC.UnpauseGame();
        CGSC.LoadScene(CGSC.QuestNames[0], true, () => {
            //Debug.Log("Levek select");
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.SetCurrentMenu(1);
            menuManager.LevelSelect();
        });
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
