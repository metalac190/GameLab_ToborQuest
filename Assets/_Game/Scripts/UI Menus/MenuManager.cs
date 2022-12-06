using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuType
{
    MainMenu,
    LevelSelect,
    LevelInfoMenu,
    Settings,
    QuestSelect,
	Credits
}

public class MenuManager : MonoBehaviour
{
    public List<MenuController> menuControllerList;

    public MenuType currentMenu { get; set; } = MenuType.MainMenu;

    [SerializeField] private GameObject mainMenuSelect;
    [SerializeField] private GameObject levelSelectGameObject;
    
    private MenuAnimations menuAnimations;

    public MenuAnimations MenuAnimations
    {
        get { return MenuAnimations; }
    }

    MenuType lastActiveMenu;

    private void Awake()
    {
        menuAnimations = GetComponent<MenuAnimations>();
        menuControllerList.ForEach(x => x.gameObject.SetActive(true));
        menuControllerList.ForEach(x =>
        {
            if (x.menuType == MenuType.LevelInfoMenu)
                x.gameObject.SetActive(false);          
        });
        LeanTween.reset();
    }

    private void Start()
    {
        //SetActiveMenu(currentMenu);
        SetCurrentButtonSelect(mainMenuSelect);
    }

    //This makes it so I can change current menu in inspector and debug
    private void Update()
    {
        if (currentMenu == lastActiveMenu)
            return;

        lastActiveMenu = currentMenu;
        //SetActiveMenu(currentMenu);
    }

    private void SetActiveMenu(MenuType value)
    {
        //Debug.Log("deactivate menu");
        //menuControllerList.ForEach(x =>
        //{
        //    if (x.menuType != value)
        //        x.gameObject.SetActive(false);
        //    else
        //        x.gameObject.SetActive(true);
        //});
        lastActiveMenu = value;
    }

    public void SetCurrentButtonSelect(GameObject value)
    {
        if (!value) return;
        CGSC.MouseKeyboardManager.UpdateSelected(value);
    }

    public void SetCurrentMenu(int value)
    {
        MenuType tempMenu = (MenuType)value;
        switch (tempMenu)
        {
            case (MenuType.MainMenu):
                if (menuAnimations.AnimatorController.GetBool("QuestSelect"))
                {
                    menuAnimations.QuestSelect(false);
                }
                else if (menuAnimations.AnimatorController.GetBool("SettingsMenu"))
                {
                    menuAnimations.SettingsMenu(false);
                }
				else
				{
					menuAnimations.Credits(false, () =>
					{
						currentMenu = tempMenu;
					});
				}
                break;
            case (MenuType.LevelSelect):
                if (menuAnimations.AnimatorController.GetBool("LevelSelect"))
                {
                    menuAnimations.LevelInfoMenu(false);
                }
                else
                {
                    menuAnimations.LevelSelect(true);
                }
                break;
            case (MenuType.LevelInfoMenu):
                menuAnimations.LevelInfoMenu(true);
                break;
            case (MenuType.Settings):
                menuAnimations.SettingsMenu(true);
                break;
            case (MenuType.QuestSelect):
                if (menuAnimations.AnimatorController.GetBool("LevelSelect"))
                {
                    menuAnimations.LevelSelect(false);
                }
                else
                {
                    menuAnimations.QuestSelect(true);
                }
                break;
			case (MenuType.Credits):
				menuAnimations.Credits(true);
				break;
		}            
        currentMenu = tempMenu;
    }

    public void ChangeScene(string value)
    {
        CGSC.LoadScene(value, true, true);
    }

    public void QuestSelect()
    {
        menuAnimations.QuestSelect(true);
    }

    public void LevelSelect() => SetCurrentButtonSelect(levelSelectGameObject);

    public void StartLevelSelect()
    {
        menuAnimations.StartLevelSelectMenu();
        currentMenu = MenuType.LevelSelect;
    }

    public void ExitGame()
    {
        CGSC.QuitGame();
    }
}
