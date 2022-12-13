using System;
using System.Collections;
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
    
	private bool _inGame;
    
	public static bool InGame;
	private static bool CanUpdate => !CreditsRoll.CreditsActive && !InGame;
    
    private static bool _usingMouse;
	[SerializeField, ReadOnly] private GameObject _currentlySelected;
	[SerializeField] private float _ignoreMouseTime = 0.1f;
    
    private InputSystemUIInputModule _inputSystem;
    private EventSystem _eventSystem;

	private static bool _wasController = true;
	
	private bool _ignoreMouseMovement;

    private void Awake()
	{
		InGame = false;
        instance = this;
        FindReferences();
    }
    
	private void Start()
	{
		SetUsingMouse(!_wasController);
		if (_wasController) Mouse.current.WarpCursorPosition(Vector2.zero);
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
		if (_inGame != InGame)
		{
			_inGame = InGame;
			if (_inGame)
			{
				_currentlySelected = null;
				SetUsingMouse(false);
				_usingMouse = false;
				Mouse.current.WarpCursorPosition(Vector2.zero);
			}
		}
    }

    private void ReturnToKeyboardController(InputAction.CallbackContext context)
	{
		if (!CanUpdate) return;
		_wasController = true;
		if (!_usingMouse) return;
		Mouse.current.WarpCursorPosition(Vector2.zero);
		StartCoroutine(IgnoreMouseMovement());
	    SetUsingMouse(false);
	}
    
	private IEnumerator IgnoreMouseMovement()
	{
		if (_ignoreMouseMovement) yield break;
		_ignoreMouseMovement = true;
		for (float t = 0; t < _ignoreMouseTime; t += Time.deltaTime)
		{
			yield return null;
		}
		_ignoreMouseMovement = false;
	}

    private void OnMouseMovement(InputAction.CallbackContext context)
	{
		if (!CanUpdate || _ignoreMouseMovement) return;
		if (_wasController)
		{
			var center = new Vector2(Screen.width, Screen.height);
			Mouse.current.WarpCursorPosition(center * 0.5f);
			_wasController = false;
			return;
		}
		if (_usingMouse) return;
		SetUsingMouse(true);
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
