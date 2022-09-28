using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuType
{
    MainMenu,
    LevelSelect,
    HUD,
    Settings
}

public class MenuManager : MonoBehaviour
{
    public List<MenuController> menuControllerList;

    public MenuType currentMenu { get; set; } = MenuType.MainMenu;

    public GameObject firstMenuObject;

    MenuType lastActiveMenu;

    private void Awake()
    {
        menuControllerList.ForEach(x => x.gameObject.SetActive(true));
        SetActiveMenu(currentMenu);
        SetCurrentButtonSelect(firstMenuObject);
    }
    //This makes it so I can change current menu in inspector and debug
    private void Update()
    {
        if (currentMenu == lastActiveMenu)
            return;

        SetActiveMenu(currentMenu);
    }

    private void SetActiveMenu(MenuType value)
    {
        menuControllerList.ForEach(x =>
        {
            if (x.menuType != value)
                x.gameObject.SetActive(false);
            else
                x.gameObject.SetActive(true);
        });
        lastActiveMenu = value;
    }

    public void SetCurrentButtonSelect(GameObject value)
    {
        EventSystem.current.SetSelectedGameObject(value);
    }

    public void SetCurrentMenu(int value)
    {
        currentMenu = (MenuType)value;
    }

    public void ChangeScene(string value)
    {
        // Notes from Brandon: Switched to using CGSC!
        // Probably switch to Async Later
        CGSC.LoadScene(value);
    }

    public void ExitGame()
    {
        CGSC.QuitGame();
    }
}
