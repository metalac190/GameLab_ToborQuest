using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public HUDManager hudManager;
    [SerializeField] private LevelDataObject levelDataObj;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private MedalUIHelper medalHelper;
    //[SerializeField] private TextMeshProUGUI levelNameText;

    public GameObject QuestionBox;

    public TextMeshProUGUI currentTime;

    [SerializeField] private TextMeshProUGUI goalTimeText;
    [SerializeField] private Image goalMedalImage;

    [SerializeField] private Button ResumeButton;
    private void Awake()
    {
        QuestionBox.SetActive(false);
        goalTimeText.text = levelDataObj.NextGoalTimeFormatted;
        medalHelper.SetMedalUI(goalMedalImage, levelDataObj.NextGoalMedal);
        //levelNameText.text = levelInfoObj.GetLevelSceneName();
    }

    private void OnEnable()
    {        
        currentTime.text = hudManager.GetCurrentTimeText();
        levelNameText.text = levelDataObj.LevelName;
    }

    private void Update()
    {
        if (!CGSC.Paused)
            return;

        if (!(Keyboard.current.backspaceKey.wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame))
            return;

        ResumeButton.onClick.Invoke();
    }

    //public void UnPause()
    //{
    //    //Time.timeScale = 1;
    //    CGSC.TogglePauseGame();
    //}

    public void ReturnToMainMenu()
    {
        //SceneManager.LoadScene(value);
        CGSC.UnpauseGame();
        CGSC.LoadMainMenu(true, true);     
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
