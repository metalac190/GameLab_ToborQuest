using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    private MovementInputs _movementInputs;

    private Vector2 _directionVector;
    public Vector2 DirectionVector => _directionVector;

    private float _speed;
    public float Speed => _speed;

    private bool _drift;
    public bool Drift => _drift;

    private void Awake()
    {
        _movementInputs = new MovementInputs();
    }

    private void OnEnable()
    {
        _movementInputs.Enable();
    }

    private void OnDisable()
    {
        _movementInputs.Disable();
    }

    private void Update()
    {
        _directionVector = _movementInputs.Player.Move.ReadValue<Vector2>();
        _speed = _movementInputs.Player.Drive.ReadValue<float>();
        Debug.Log(_speed);
        _drift = _movementInputs.Player.Drift.IsPressed();
    }
}
