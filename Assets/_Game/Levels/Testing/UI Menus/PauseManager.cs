using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    public void ChangeScene(string value)
    {
        SceneManager.LoadScene(value);
    }

    public void SetCurrentSelected(GameObject value)
    {
        EventSystem.current.SetSelectedGameObject(value);
    }

}
