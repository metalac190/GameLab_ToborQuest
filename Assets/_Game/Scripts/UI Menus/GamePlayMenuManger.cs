using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class GamePlayMenuManger : MonoBehaviour
{
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject pauseFirstButton;

    // Start is called before the first frame update
    void Awake()
    {
        CGSC.GameOver = false;
        hudPanel.SetActive(true);
        CGSC.UnpauseGame();
    }

    private void OnEnable()
    {
        CGSC.OnPause += PauseGame;
        CGSC.OnUnpause += UnpauseGame;
    }

    private void OnDisable()
    {
        CGSC.OnPause -= PauseGame;
        CGSC.OnUnpause -= UnpauseGame;
    }

    private void PauseGame()
    {
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void UnpauseGame()
    {
        pausePanel.SetActive(false);
        //CGSC.TogglePauseGame();
    }

    public void ButtonUnPause()
    {
        pausePanel.SetActive(false);
        CGSC.TogglePauseGame();
    }


}
