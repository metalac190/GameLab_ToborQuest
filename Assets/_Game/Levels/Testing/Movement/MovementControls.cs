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
    }

    private void OnDisable()
    {
        _movementInput.Disable();
    }

    private void Update()
    {
        _directionVector = _movementInput.Player.Direction.ReadValue<Vector2>();
        _speed = _movementInput.Player.Drive.ReadValue<float>();
        _drift = _movementInput.Player.Drift.IsPressed();
        _boost = _movementInput.Player.Boost.IsPressed();
        _sideFlip = _movementInput.Player.SideFlip.ReadValue<float>();
    }
}
