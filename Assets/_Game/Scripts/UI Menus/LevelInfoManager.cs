using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LevelInfoManager : MonoBehaviour
{
    public Image levelPreviewImage;
    public TextMeshProUGUI levelDescriptionText;

    public Button onStart;
    private LevelInfoObject levelInfoObj;

    private void SetLevelInfoObject(LevelInfoObject value)
    {
        levelInfoObj = value;
    }

    private void OnEnable()
    {
        SetInfo(levelInfoObj);
    }

    private void SetInfo(LevelInfoObject value)
    {
        
    }

}
