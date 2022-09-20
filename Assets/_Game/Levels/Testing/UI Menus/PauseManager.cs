using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public HUDManager hudManager;

    public GameObject QuestionBox;

    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI bestTime;

    private void Awake()
    {
        QuestionBox.SetActive(false);
    }

    private void OnEnable()
    {
        currentTime.text = hudManager.GetCurrentTimeText();
        bestTime.text = hudManager.GetBestTimeString();
    }

    public void UnPause()
    {
        Time.timeScale = 1;        
    }
}
