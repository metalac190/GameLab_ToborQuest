using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuQuitListener : MonoBehaviour
{
    private MenuManager _menuManager;
    [SerializeField]
    private Button settingBackButton, levelSelectBackButton, levelInfoBackButton;
    
    void Awake()
    {
        _menuManager = gameObject.GetComponent<MenuManager>();        
    }
   
    void Update()
    {
        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
        {
            HandleExitMenu();
            //Debug.Log("Exit " + _menuManager.currentMenu.ToString());
        }
    }

    private void HandleExitMenu()
    {
        switch (_menuManager.currentMenu)
        {
            case MenuType.Settings:
                settingBackButton.onClick.Invoke();
                break;
            case MenuType.LevelSelect:
                levelSelectBackButton.onClick.Invoke();
                break;
            case MenuType.LevelInfoMenu:
                levelInfoBackButton.onClick.Invoke();
                break;
        }
    }

}
