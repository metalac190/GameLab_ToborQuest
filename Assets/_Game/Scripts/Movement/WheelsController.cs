using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WheelsController : MonoBehaviour
{
    [SerializeField] private List<Wheel> _wheels = new List<Wheel>();

    [Header("Collisions")]
    [SerializeField] private LayerMask _wallLayer;

    [Header("Friction")]
    [SerializeField] private float _standardWheelDampeningRate = 0.25f;
    [SerializeField] private float _standardFrictionStiffness = 1f;

    [Header("Debug")]
    [SerializeField, ReadOnly] private float _currentWheelDampeningRate;
    [SerializeField, ReadOnly] private float _currentFrictionStiffness;

    public float StandardDampeningRate => _standardWheelDampeningRate;
    public float StandardFrictionStiffness => _standardFrictionStiffness;

    private MovementController _mc;
    private MovementControls _input;
    private Rigidbody _rb;

    private void Start()
    {
        _mc = GetComponent<MovementController>();
        _input = GetComponent<MovementControls>();
        _rb = GetComponent<Rigidbody>();

        SetWheelFriction(_standardWheelDampeningRate, _standardFrictionStiffness);
    }

    private void FixedUpdate()
    {
        UpdateWheels();
    }

    public void SetWheelFriction(float wheelDampening = 0, float frictionStiffness = 0)
    {
            _currentWheelDampeningRate = wheelDampening;
            _currentFrictionStiffness = frictionStiffness;

            UpdateWheelFriction();
    }
    
    private void UpdateWheelFriction()
    {
        foreach (var w in _wheels)
        {
            w._wheelCollider.wheelDampingRate = _currentWheelDampeningRate;

            var ffrictionCurve = w._wheelCollider.forwardFriction;
            ffrictionCurve.stiffness = _currentFrictionStiffness;
            w._wheelCollider.forwardFriction = ffrictionCurve;

            var sfrictionCurve = w._wheelCollider.sidewaysFriction;
            sfrictionCurve.stiffness = _currentFrictionStiffness;
            w._wheelCollider.sidewaysFriction = sfrictionCurve;
        }
    }
    

    private void UpdateWheels()
    {

        foreach (var w in _wheels)
        {
            w.Steer(_input.DirectionVector.x);
            w.UpdatePosition();
        }
    }

    public bool WheelsGroundCheck()
    {
        foreach (var w in _wheels)
        {
            if (w._wheelCollider.isGrounded) return true;
        }
        return false;
    }
}
