using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelInfoManager : MonoBehaviour
{
    public Image levelTitleImage;
    public Image levelNameImage;
    public Image levelPreviewImage;
    public TextMeshProUGUI levelDescriptionText;
    public Button onStart;
    public Button onBack;
    private GameObject backLevelButton;
    private LevelInfoObject levelInfoObj;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    [SerializeField] private MedalUIHelper medalHelper;
    [SerializeField] private TextMeshProUGUI currentMedalStatusText;
    [SerializeField] private Image currentMedalImage;
    [SerializeField] private Image nextMedalGoalImage;
    [SerializeField] private TextMeshProUGUI nextGoalTimeText;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void SetLevelInfoObject(LevelInfoObject value)
    {
        levelInfoObj = value;
        //this.gameObject.SetActive(true);
        //Debug.Log("Activating " + transform.name);
        EventSystem.current.SetSelectedGameObject(onStart.gameObject);
    }

    public void SetBackLevelButton(GameObject value)
    {        
        backLevelButton = value;
    }

    private void OnEnable()
    {
        if (!levelInfoObj)
            return;
        SetInfo(levelInfoObj);
        onBack.onClick.AddListener(() =>
        {
            EventSystem.current.SetSelectedGameObject(backLevelButton);
        });
    }

    private void SetInfo(LevelInfoObject value)
    {
        levelTitleImage.sprite = levelInfoObj.levelTitleSprite;
        levelNameImage.sprite = levelInfoObj.levelNameSprite;
        levelPreviewImage.sprite = levelInfoObj.levelInfoPreviewSprite;
        levelDescriptionText.text = levelInfoObj.levelDecription;
        bestTimeText.text = levelInfoObj.GetBestTimeFormatted();

        currentMedalStatusText.text = levelInfoObj.CurrentMedal.ToString();
        medalHelper.SetMedalUI(currentMedalImage, levelInfoObj.CurrentMedal);
        medalHelper.SetMedalUI(nextMedalGoalImage, levelInfoObj.GetNextMedalGoal());
        nextGoalTimeText.text = levelInfoObj.GetNextTimeGoalFormatted();

        onStart.onClick.AddListener(() =>
        {
            CGSC.LoadScene(value.GetLevelSceneName());
        });
    }
}
