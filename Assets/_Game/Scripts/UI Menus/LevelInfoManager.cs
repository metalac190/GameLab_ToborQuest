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
    private LevelDataObject levelDataObj;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    [SerializeField] private MedalUIHelper medalUIHelper;
    [SerializeField] private TextMeshProUGUI currentMedalStatusText;
    [SerializeField] private Medal3DObjectHelper medal3DHelper;
    [SerializeField] private Image nextMedalGoalImage;
    [SerializeField] private TextMeshProUGUI nextGoalTimeText;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void SetLevelDataObject(LevelDataObject value)
    {
        levelDataObj = value;
        SetInfo();
        //this.gameObject.SetActive(true);
        //Debug.Log("Activating " + transform.name);
        EventSystem.current.SetSelectedGameObject(onStart.gameObject);
    }

    public void SetBackLevelButton(GameObject value)
    {        
        backLevelButton = value;
        onBack.onClick.RemoveAllListeners();    
        onBack.onClick.AddListener(() =>
        {
            EventSystem.current.SetSelectedGameObject(backLevelButton);
        });
    }

    private void SetInfo()
    {
        levelTitleImage.sprite = levelDataObj.levelTitleSprite;
        levelNameImage.sprite = levelDataObj.levelNameSprite;
        levelPreviewImage.sprite = levelDataObj.levelInfoPreviewSprite;
        levelDescriptionText.text = levelDataObj.levelDecription;
        bestTimeText.text = levelDataObj.BestTimeFormatted;

        currentMedalStatusText.text = levelDataObj.CurrentMedal.ToString();
        //medalHelper.SetMedalUI(currentMedalImage, levelInfoObj.CurrentMedal);
        medal3DHelper.SetMedal(levelDataObj.CurrentMedal);
        medalUIHelper.SetMedalUI(nextMedalGoalImage, levelDataObj.NextGoalMedal);
        nextGoalTimeText.text = levelDataObj.NextGoalTimeFormatted;

        onStart.onClick.RemoveAllListeners();
        onStart.onClick.AddListener(() =>
        {
            CGSC.PlayingQuest = false;
            CGSC.LoadScene(levelDataObj.GetLevelSceneName(),true,true);
        });
    }
}
