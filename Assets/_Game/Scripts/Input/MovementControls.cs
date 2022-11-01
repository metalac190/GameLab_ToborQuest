using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControls : MonoBehaviour
{
    public MovementInput _movementInput;

    private Vector2 _directionVector;
    public Vector2 DirectionVector => _directionVector;

    private float _speed;
    public float Speed => _speed;

    private bool _drift;
    public bool Drift => _drift;

    private bool _boost;
    public bool Boost => _boost;

    private float _sideFlip;
    public float SideFlip => _sideFlip;
    

    private void Awake()
    {
        _movementInput = new MovementInput();
    }

    private void OnEnable()
    {
        _movementInput.Enable();
        _movementInput.Player.TogglePaused.performed += CGSC.TogglePauseGame;
        _movementInput.Player.Reset.performed += CGSC.RestartLevel;
        _movementInput.Player.SkipDialogue.performed += DialogueSystem.SkipDialogueStatic;
    }

    private void OnDisable()
    {
        _movementInput.Player.TogglePaused.performed -= CGSC.TogglePauseGame;
        _movementInput.Player.Reset.performed -= CGSC.RestartLevel;
        _movementInput.Player.SkipDialogue.performed -= DialogueSystem.SkipDialogueStatic;
        _movementInput.Disable();
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
    }

    private void TogglePausedGame(InputAction.CallbackContext context)
    {
        
    }
}
