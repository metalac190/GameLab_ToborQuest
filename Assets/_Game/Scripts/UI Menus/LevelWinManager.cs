using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelWinManager : MonoBehaviour
{
    private HUDManager hudManager;
    [SerializeField]
    private TextMeshProUGUI winText;
    [SerializeField]
    private GameObject returnLevelSelectButton;
    private GameObject _children;
    private string levelSaveTimeName;//"Level1BestTime";
    public string LevelSaveName { get { return levelSaveTimeName; } }

    private void Awake()
    {
        //Find hudmanager, should only be 1 in the scene.
        hudManager = GameObject.FindObjectOfType<HUDManager>(true);
        levelSaveTimeName = SceneManager.GetActiveScene().name + "BestTime";
        //Debug.Log(levelSaveTimeName);
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
        transform.GetChild(0).gameObject.SetActive(false);
        //this.transform.GetChild();
    }

    private void WinGamePanel()
    {
        hudManager.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        winText.text = "DELIVERY TIME: " + hudManager.GetCurrentTimeText();
        hudManager.currentTimerText.StopTime = true;
        EventSystem.current.SetSelectedGameObject(returnLevelSelectButton);
        SaveBestTime();
    }

    public void ContinueNextLevel()
    {
        //CGSC.LoadScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name, true);
        CGSC.LoadNextSceneRaw();
    }

    public void RestartLevel()
    {
        CGSC.RestartLevel();
    }

    public void ReturnToLevels()
    {
        CGSC.LoadMainMenu(true, () => {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
            //menuManager.SetCurrentMenu(1);
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
