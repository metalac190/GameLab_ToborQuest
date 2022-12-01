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
    private TextMeshProUGUI deliveryTimeText;
    [SerializeField] private LevelDataObject levelDataObj;
    [SerializeField] private MedalUIHelper medalHelper;

    [SerializeField] private Image levelCompleteImage;

    [Header("New Best Time")]
    [SerializeField] private GameObject newBestTimeObjects;
    [SerializeField] private TextMeshProUGUI newMedalText;
    [SerializeField] private Medal3DObjectHelper medal3DHelper;
    [SerializeField] private TextMeshProUGUI newBestTimeText;

    [Header("Next Goal")]
    [SerializeField] private TextMeshProUGUI nextGoalTimeText;
    [SerializeField] private Image nextGoalMedalImage;

    [Header("Quest")]
    [SerializeField] private GameObject questSelected;
    [SerializeField] private List<GameObject> questObjs;
    [SerializeField] private GameObject notQuestSelected;
    [SerializeField] private List<GameObject> notQuestObjs;
    
    private GameObject _children;
    public string LevelSaveName { get; private set; }
    public LevelDataObject LevelDataObject => levelDataObj;

    private void Awake()
    {
        //Find hudmanager, should only be 1 in the scene.
        hudManager = GameObject.FindObjectOfType<HUDManager>(true);
        LevelSaveName = levelDataObj.GetLevelSceneName() + "BestTime";
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
        bool playingQuest = CGSC.PlayingQuest;
        foreach (var obj in questObjs)
        {
            obj.SetActive(playingQuest);
        }
        foreach (var obj in notQuestObjs)
        {
            obj.SetActive(!playingQuest);
        }
        
        hudManager.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        levelCompleteImage.sprite = levelDataObj.levelCompleteSprite;
        deliveryTimeText.text = "DELIVERY TIME: " + hudManager.GetCurrentTimeText();
        TimerUI.startTimer = false;
        EventSystem.current.SetSelectedGameObject(playingQuest ? questSelected : notQuestSelected);
        Cursor.lockState = CursorLockMode.None;
        SaveBestTime();
        ShowNextGoal();
    }

    public void ContinueNextLevel()
    {
        //CGSC.LoadScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name, true);
        //CGSC.UnpauseGame();
        CGSC.LoadNextSceneRaw(true, true);
    }

    public void RestartLevel()
    {
        CGSC.RestartLevel();
    }

    public void ReturnToLevels()
    {
        //CGSC.UnpauseGame();
        CGSC.LoadMainMenu(true, true, () =>
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
            menuManager.LevelSelect();
        });
    }

    private void SaveBestTime()
    {
        float currentTime = hudManager.currentTimerText.timeRemaining;
        CGSC.TotalTime += currentTime;
        TimerUI.levelTime = 0;

        if (levelDataObj.TrySetBestTime(currentTime))
        {
            // new best time
            newBestTimeObjects.SetActive(true);
            newBestTimeText.text = "Best time: " + levelDataObj.BestTimeFormatted;
            newMedalText.text = "You Earned " + levelDataObj.CurrentMedal;
        }
        else
        {
            // no new best time
            newBestTimeObjects.SetActive(false);
        }

        medal3DHelper.SetMedal(levelDataObj.CurrentMedal);
    }

    private void ShowNextGoal() {
        levelDataObj.SetNewGoal(); //shouldn't have to call this here, but I do so whatever
        nextGoalTimeText.text = "Next Goal: " + levelDataObj.NextGoalTimeFormatted;
        medalHelper.SetMedalUI(nextGoalMedalImage, levelDataObj.NextGoalMedal);
    }
}
