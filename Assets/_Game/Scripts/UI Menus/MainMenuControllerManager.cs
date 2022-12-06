using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainMenuControllerManager : MonoBehaviour
{
    private static MainMenuControllerManager instance;
    public static MainMenuControllerManager Instance
    {
        get
        {
            if (instance) return instance;
            instance = FindObjectOfType<MainMenuControllerManager>();
            if (!instance)
            {
                var inputSystem = FindObjectOfType<InputSystemUIInputModule>();
                instance = inputSystem.gameObject.AddComponent<MainMenuControllerManager>();
            }
            instance.FindReferences();
            return instance;
        }
    }
    
    private static bool _usingMouse;
    [SerializeField, ReadOnly] private GameObject _currentlySelected;
    
    private InputSystemUIInputModule _inputSystem;
    private EventSystem _eventSystem;

	private bool _firstMouse;

    private void Awake()
    {
        instance = this;
        FindReferences();
    }
    
	private void Start()
	{
		_firstMouse = false;
		SetUsingMouse(false);
	}

    private void FindReferences()
    {
        _eventSystem = EventSystem.current;
        if (!_inputSystem) _inputSystem = GetComponent<InputSystemUIInputModule>();
        if (!_inputSystem) FindObjectOfType<InputSystemUIInputModule>();
    }

    private void OnEnable()
    {
        _inputSystem.move.action.performed += ReturnToKeyboardController;
        _inputSystem.point.action.performed += OnMouseMovement;
    }
    
    private void OnDisable()
    {
        _inputSystem.move.action.performed -= ReturnToKeyboardController;
        _inputSystem.point.action.performed -= OnMouseMovement;
    }

    private void Update()
    {
        if (!_usingMouse)
        {
            var obj = _eventSystem.currentSelectedGameObject;
            if (obj) _currentlySelected = obj;
        }
    }

    private void ReturnToKeyboardController(InputAction.CallbackContext context)
	{
		if (!_usingMouse) return;
	    SetUsingMouse(false);
	    _firstMouse = false;
	    Mouse.current.WarpCursorPosition(Vector2.zero);
    }

    private void OnMouseMovement(InputAction.CallbackContext context)
	{
		if (_usingMouse) return;
        if (!_firstMouse)
        {
            _firstMouse = true;
            return;
        }
	    SetUsingMouse(true);
	    var center = new Vector2(Screen.width, Screen.height);
	    Mouse.current.WarpCursorPosition(center * 0.5f);
    }

    private void SetUsingMouse(bool usingMouse, bool setSelected = true)
    {
        _usingMouse = usingMouse;
        Cursor.visible = usingMouse;
        if (setSelected) SetSelected(usingMouse ? null : _currentlySelected);
    }

    private void SetSelected(GameObject obj)
    {
        _eventSystem.SetSelectedGameObject(obj);
    }

    public void UpdateSelected(GameObject obj)
    {
        if (obj) _currentlySelected = obj;
        if (!_usingMouse) SetSelected(obj);
    }
}
