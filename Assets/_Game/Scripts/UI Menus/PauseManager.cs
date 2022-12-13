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
    [SerializeField] private BasicPanel _settingsPanel;
    [SerializeField] private GameObject _confirmExit;
    [SerializeField] private CanvasGroup _buttonGrp;

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
        _settingsPanel.SetGroupActive(false);
        _confirmExit.SetActive(false);
	    _buttonGrp.interactable = true;
	    MainMenuControllerManager.InGame = false;
    }
    
	private void OnDisable()
	{
		MainMenuControllerManager.InGame = true;
	}
    
	private void OnDestroy()
	{
		MainMenuControllerManager.InGame = false;
	}

    private void Update()
    {
        if (!CGSC.Paused)
            return;
        if ((Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame) ||
                (Keyboard.current != null && Keyboard.current.backspaceKey.wasPressedThisFrame))
        {
            ResumeButton.onClick.Invoke();
        }            
    }

    public void UnPause()
    {
        //Time.timeScale = 1;
        //gameObject.SetActive(false);
        CGSC.UnpauseGame();
    }

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
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
            menuManager.LevelSelect();
        });
    }

    public void SetCurrentSelected(GameObject value)
    {
        CGSC.MouseKeyboardManager.UpdateSelected(value);
    }

}
