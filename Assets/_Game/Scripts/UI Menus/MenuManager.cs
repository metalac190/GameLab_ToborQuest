using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuType
{
    MainMenu,
    LevelSelect,
    LevelInfoMenu,
    Settings
}

public class MenuManager : MonoBehaviour
{
    public List<MenuController> menuControllerList;

    public MenuType currentMenu { get; set; } = MenuType.MainMenu;

    [SerializeField]
    private GameObject mainMenuSelect;
    [SerializeField]
    private GameObject levelSelectGameObject;
    private MenuAnimations menuAnimations;

    MenuType lastActiveMenu;

    private void Awake()
    {
        menuAnimations = GetComponent<MenuAnimations>();
        menuControllerList.ForEach(x => x.gameObject.SetActive(true));
        SetActiveMenu(currentMenu);
        SetCurrentButtonSelect(mainMenuSelect);
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
        MenuType tempMenu = (MenuType)value;
        switch (tempMenu)
        {
            case (MenuType.MainMenu):
                menuAnimations.LevelSelect(false, () =>
                {                    
                    currentMenu = tempMenu;
                });
                break;
            case (MenuType.LevelSelect):
                if (menuAnimations.AnimatorController.GetBool("LevelSelect"))
                {
                    menuAnimations.LevelInfoMenu(false, () =>
                    {
                        currentMenu = tempMenu;
                    });
                }
                else
                {
                    menuAnimations.LevelSelect(true, () =>
                    {
                        currentMenu = tempMenu;
                    });
                }
                break;
            case (MenuType.LevelInfoMenu):
                menuAnimations.LevelInfoMenu(true, () =>
                {
                    currentMenu = tempMenu;
                });
                break;
        }            
    }

    public void ChangeScene(string value)
    {
        // Notes from Brandon: Switched to using CGSC!
        // Probably switch to Async Later
        CGSC.LoadScene(value);
    }

    public void LevelSelect()
    {
        EventSystem.current.SetSelectedGameObject(levelSelectGameObject);
    }

    public void StartLevelSelect()
    {
        Debug.Log(currentMenu);
        //menuAnimations.AnimatorController.Play("LevelSelect");
        //currentMenu = MenuType.LevelSelect;
    }

    public void ExitGame()
    {
        CGSC.QuitGame();
    }
}
