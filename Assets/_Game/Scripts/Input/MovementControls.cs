using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControls : MonoBehaviour
{
    [SerializeField] private CheckpointTracker _checkpoint;
    [SerializeField, ReadOnly] private InputControlScheme _activeControlScheme;
    public MovementInput _movementInput;
    private PlayerInput _playerInput;

    private Vector2 _directionVector;
    public Vector2 DirectionVector => _directionVector;

    [SerializeField, ReadOnly] private float _speed;
    public float Speed => _speed;

    [SerializeField, ReadOnly] private bool _drift;
    public bool Drift => _drift;

    [SerializeField, ReadOnly] private bool _boost;
    public bool Boost => _boost;

    [SerializeField, ReadOnly] private float _sideFlip;
    public float SideFlip => _sideFlip;

    public InputControlScheme ACtiveControlScheme => _activeControlScheme;

	private bool _questRespawn;
	private bool _canRestart = true;
    

    private void Awake()
    {
        _movementInput = new MovementInput();
	    _playerInput = GetComponent<PlayerInput>();
	    _canRestart = true;
    }

    private void OnEnable()
    {
        _movementInput.Enable();
        _movementInput.Player.TogglePaused.performed += CGSC.TogglePauseGame;
        _movementInput.Player.Reset.performed += Restart;
	    _movementInput.Player.SkipDialogue.performed += DialogueSystem.SkipDialogueStatic;
	    CGSC.OnWinGame += LockRestart;
    }

    private void OnDisable()
    {
        _movementInput.Player.TogglePaused.performed -= CGSC.TogglePauseGame;
        _movementInput.Player.Reset.performed -= Restart;
        _movementInput.Player.SkipDialogue.performed -= DialogueSystem.SkipDialogueStatic;
	    _movementInput.Disable();
	    CGSC.OnWinGame -= LockRestart;
    }
    
	private void LockRestart()
	{
		_canRestart = false;
	}

    private void Restart(InputAction.CallbackContext context) => Restart();
    [Button]
    private void Restart()
	{
        if (CGSC.PlayingQuest)
        {
	        if (!_canRestart) return;
            if (TimerUI.startTimer)
            {
                _questRespawn = true;
                TimerUI.startTimer = false;
            }
            StartCoroutine(_checkpoint.Respawn());
        }
        else
        {
            CGSC.RestartLevel();
        }
    }

    // Note From Brandon: This does not always happen before the Movement Controller's script, Update Loops happen between
    // objects in a random order. Only causes one frame of Input Lag (and that only sometimes) but figured I would mention lol
    // Using Events, like I added in OnEnable / OnDisable, happen immediately before any Update. Dw its not too important
    private void Update()
    {
        // Input will need to be disabled when CGSC.Paused
        _directionVector = _movementInput.Player.Direction.ReadValue<Vector2>();
        _speed = _movementInput.Player.Drive.ReadValue<float>();
        _drift = _movementInput.Player.Drift.IsPressed();
        _boost = _movementInput.Player.Boost.IsPressed();
        _sideFlip = _movementInput.Player.SideFlip.ReadValue<float>();

        if (_questRespawn && (_directionVector != Vector2.zero || _speed != 0))
        {
            TimerUI.startTimer = true;
            _questRespawn = false;
        }

        _activeControlScheme = (InputControlScheme) _playerInput.user.controlScheme;
    }
}
