using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public HUDManager hudManager;
    [SerializeField] private LevelInfoObject levelInfoObj;
    [SerializeField] private MedalUIHelper medalHelper;
    [SerializeField] private TextMeshProUGUI levelNameText;

    public GameObject QuestionBox;

    public TextMeshProUGUI currentTime;

    [SerializeField] private TextMeshProUGUI goalTimeText;
    [SerializeField] private Image goalMedalImage;

    private void Awake()
    {
        QuestionBox.SetActive(false);
        levelInfoObj.GetBestTimeFormatted(); //just an easy way to set the BestTime
        goalTimeText.text = levelInfoObj.GetNextTimeGoalFormatted();
        medalHelper.SetMedalUI(goalMedalImage, levelInfoObj.GetNextMedalGoal());
        levelNameText.text = levelInfoObj.GetLevelSceneName();
    }

    private void OnEnable()
    {        
        currentTime.text = hudManager.GetCurrentTimeText();
    }

    //public void UnPause()
    //{
    //    //Time.timeScale = 1;
    //    CGSC.TogglePauseGame();
    //}

    public void ReturnToMainMenu()
    {
        //SceneManager.LoadScene(value);
        CGSC.LoadMainMenu();        
    }

    public void Restart()
    {
        CGSC.RestartLevel();
    }

    public void ReturnToLevels()
    {
        CGSC.UnpauseGame();
        CGSC.LoadMainMenu(true, true,() =>
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
            menuManager.LevelSelect();
        });
    }

    public void SetCurrentSelected(GameObject value)
    {
        EventSystem.current.SetSelectedGameObject(value);
    }

}
