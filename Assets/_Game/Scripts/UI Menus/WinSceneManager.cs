using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startSelect;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(startSelect);
    }


    public void LevelReturn()
    {
        CGSC.LoadMainMenu(true, true, () =>
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.StartLevelSelect();
            menuManager.LevelSelect();
        });
    }

    public void Quit()
    {
        CGSC.LoadMainMenu(true, true);
    }

}
