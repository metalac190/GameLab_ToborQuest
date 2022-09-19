using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuType
{
    MainMenu,
    LevelSelect,
    HUD,
    Settings,
    Pause,
}

public class MenuManager : MonoBehaviour
{

    public List<MenuController> menuControllerList;

    public MenuType currentMenu = MenuType.MainMenu;

    MenuType lastActiveMenu;

    private void Awake()
    {
        menuControllerList.ForEach(x => x.gameObject.SetActive(true));
        SetActiveMenu(currentMenu);
    }
    //This makes it so I can change current menu in inspector and debug
    private void Update()
    {
        if (currentMenu == lastActiveMenu)
            return;

        SetActiveMenu(currentMenu);
    }

    public void SetActiveMenu(MenuType value)
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
}
