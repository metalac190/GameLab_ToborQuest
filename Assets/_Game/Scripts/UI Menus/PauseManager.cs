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
        CGSC.GameOver = false;
        currentTime.text = hudManager.GetCurrentTimeText();
        bestTime.text = hudManager.GetBestTimeString();
    }

    //public void UnPause()
    //{
    //    //Time.timeScale = 1;
    //    CGSC.TogglePauseGame();
    //}

    public void ReturnToMainMenu()
    {
        //SceneManager.LoadScene(value);
        CGSC.LoadMainMenu();        
    }

    public void Restart()
    {
        CGSC.RestartLevel();
    }

    public void ReturnToLevels()
    {
        CGSC.UnpauseGame();
        CGSC.LoadScene("MainMenu",true, () => {
            //Debug.Log("Levek select");
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.SetCurrentMenu(1);
            menuManager.LevelSelect();
        });
    }

    public void SetCurrentSelected(GameObject value)
    {
        EventSystem.current.SetSelectedGameObject(value);
    }

}
