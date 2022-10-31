using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelInfoManager : MonoBehaviour
{
    public Image levelPreviewImage;
    public TextMeshProUGUI levelDescriptionText;
    public Button onStart;
    public Button onBack;
    private GameObject backLevelButton;
    private LevelInfoObject levelInfoObj;
    [SerializeField] private TextMeshProUGUI bestTimeText;

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
        levelPreviewImage.sprite = levelInfoObj.levelImage;
        levelDescriptionText.text = levelInfoObj.levelInfo;
        bestTimeText.text = levelInfoObj.GetTimeFormatted();
        onStart.onClick.AddListener(() =>
        {
            CGSC.LoadScene(value.GetLevelSceneName());
        });
    }
}
