using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject startSelect;
    [SerializeField] private TMP_Text deliveryTimeText;
    [SerializeField] private string deliveryTextBefore = "Delivery Time: ";

    private void Awake()
    {
        CGSC.MouseKeyboardManager.UpdateSelected(startSelect);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        CGSC.InMainMenu = true;
        var bestTime = CGSC.TotalTime;
        BestTimesSaver.TrySetBestTime(BestTime.Quest, bestTime);
        deliveryTimeText.text = deliveryTextBefore + TimerUI.ConvertTimeToText(bestTime);
    }

    public void LevelReturn()
    {
        CGSC.LoadMainMenu(true, true, () =>
        {
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
            menuManager.LevelSelect();
        });
    }

    public void Quit()
    {
        CGSC.LoadMainMenu(true, true);
    }

}
