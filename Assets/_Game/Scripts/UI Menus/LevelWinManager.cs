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
    [SerializeField] private LevelInfoObject levelInfoObj;
    [SerializeField] private MedalUIHelper medalHelper;

    [SerializeField] private Image levelCompleteImage;

    [Header("New Best Time")]
    [SerializeField] private GameObject newBestTimeObjects;
    [SerializeField] private TextMeshProUGUI newMedalText;
    [SerializeField] private Image newMedalImage;
    [SerializeField] private TextMeshProUGUI newBestTimeText;

    [Header("Next Goal")]
    [SerializeField] private TextMeshProUGUI nextGoalTimeText;
    [SerializeField] private Image nextGoalMedalImage;

    [Header("Buttons")]
    [SerializeField]
    private GameObject returnLevelSelectButton;
    private GameObject _children;
    private string levelSaveTimeName;//"Level1BestTime";
    public string LevelSaveName { get { return levelSaveTimeName; } }

    private void Awake()
    {
        //Find hudmanager, should only be 1 in the scene.
        hudManager = GameObject.FindObjectOfType<HUDManager>(true);
        levelSaveTimeName = levelInfoObj.GetLevelSceneName() + "BestTime";
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
        levelCompleteImage.sprite = levelInfoObj.levelCompleteSprite;
        deliveryTimeText.text = "DELIVERY TIME: " + hudManager.GetCurrentTimeText();
        hudManager.currentTimerText.startTimer = false;
        EventSystem.current.SetSelectedGameObject(returnLevelSelectButton);
        SaveBestTime();
        ShowNextGoal();
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
        CGSC.LoadMainMenu(true, true,() => {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
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
                //save the new best tiem to Player Prefs
                PlayerPrefs.SetFloat(levelSaveTimeName, currentTime);

                //show and update the new best time objs
                newBestTimeObjects.SetActive(true);
                newBestTimeText.text = "Best time: " + levelInfoObj.GetBestTimeFormatted();
                newMedalText.text = "You Earned " + levelInfoObj.CurrentMedal.ToString();
                medalHelper.SetMedalUI(newMedalImage, levelInfoObj.CurrentMedal);
            }
            else
            {
                //Debug.Log("Didn't beat best time");
                newBestTimeObjects.SetActive(false);
            }
        }
        else
        {
            //Debug.Log("Best time set");
            PlayerPrefs.SetFloat(levelSaveTimeName, currentTime);
            newBestTimeObjects.SetActive(false);
        }
            
    }

    private void ShowNextGoal() {
        nextGoalTimeText.text = "Next Goal: " + levelInfoObj.GetNextTimeGoalFormatted();
        medalHelper.SetMedalUI(newMedalImage, levelInfoObj.GetNextMedalGoal());
    }
}
