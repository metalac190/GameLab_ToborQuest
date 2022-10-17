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
    private LevelInfoObject levelInfoObj;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void SetLevelInfoObject(LevelInfoObject value)
    {
        levelInfoObj = value;
        this.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(onStart.gameObject);
    }

    private void OnEnable()
    {
        SetInfo(levelInfoObj);
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
