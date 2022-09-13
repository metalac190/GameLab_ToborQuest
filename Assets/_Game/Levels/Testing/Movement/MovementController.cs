using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Vector3 _centerOfMass = Vector3.zero;

    [Header("Grounded")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Movement")]
    [SerializeField] private float _acceleration = 25;
    [SerializeField] private float _maxSpeed = 25;

    [Header("Turning")]
    [SerializeField] private bool _turnWhenStopped = true;
    [SerializeField] private float _stoppedTurnSpeed = 1.0f;
    [SerializeField] private float _standardTurnSpeed = 0.75f;

    [Header("Drifting")]
    [SerializeField] private float _driftTurnSpeed = 0.25f;
    [SerializeField] private List<TrailRenderer> _driftTrails = new List<TrailRenderer>();

    [Header("Boosting")] 
    [SerializeField] private float _boostAcceleration = 30;
    [SerializeField] private float _boostMaxSpeed = 30;
    [SerializeField] private float _boostDuration = 2f;
    [SerializeField] private float _boostCooldown = 2f;
    [SerializeField] private float _boostRemaining = 2f;
    [SerializeField] private bool _boostOnCooldown = false;
    [SerializeField] private List<TrailRenderer> _boostTrails = new List<TrailRenderer>();

    [Header("Debug")]
    [SerializeField, ReadOnly] private float _currentAcceleration = 0f;
    [SerializeField, ReadOnly] private float _currentMaxSpeed = 0f;
    [SerializeField, ReadOnly] private float _currentTurnSpeed = 0f;
    [SerializeField, ReadOnly] private float _turnSmoothVel;
    [SerializeField, ReadOnly] private bool _isMoving;
    [SerializeField, ReadOnly] private bool _isGrounded;
    [SerializeField, ReadOnly] private bool _isDrifting;
    [SerializeField, ReadOnly] private bool _isBoosting;
    [SerializeField, ReadOnly] private Vector3 _direction;
    [SerializeField, ReadOnly] private Vector3 _previousVel;

    private Rigidbody _rb;
    private MovementControls _movementControls;
    private bool _driftTrailsActive;
    private bool _boostTrailsActive;

    private bool IsGrounded() => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;

        _movementControls = GetComponent<MovementControls>();

        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
        _currentTurnSpeed = _stoppedTurnSpeed;
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        if (!_isGrounded || _turnWhenStopped) Steer();

        if (_isGrounded) Movement();

        if (!_isMoving) _currentTurnSpeed = _stoppedTurnSpeed;


        if (_boostTrailsActive != _isBoosting)
        {
            _boostTrailsActive = _isBoosting;
            foreach (var trail in _boostTrails)
            {
                trail.emitting = _boostTrailsActive;
            }
        }
    }

    private void LateUpdate()
    {
        _previousVel = _rb.velocity;
    }

    private void Steer()
    {
        _direction = new Vector3(_movementControls.DirectionVector.x, 0f, _movementControls.DirectionVector.y);

        if (_direction.magnitude >= 0.1f)
        {
            
            var currentAngle = _rb.rotation.eulerAngles.y;
            //var targetAngle = currentAngle + (_direction.x - _direction.z) * Mathf.Rad2Deg;
            var targetAngle = currentAngle + Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref _turnSmoothVel, _currentTurnSpeed);
            _rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }
    }

    private void Movement()
    {
        Vector3 forceVector = transform.forward * (_currentAcceleration * _movementControls.Speed);

        if (_movementControls.Boost && !_boostOnCooldown) Boost();
        else if (_movementControls.Speed != 0) Drive();

        if (!_movementControls.Boost && _boostRemaining < _boostDuration) StartCoroutine(BoostCooldown());

        _rb.AddForce(transform.forward * (_currentAcceleration * _movementControls.Speed), ForceMode.Acceleration);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _currentMaxSpeed);

        _isMoving = _rb.velocity.magnitude > 0.05f;

        Drift();

    }

    private void Drive()
    {
        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
    }

    private void Boost()
    {
        _currentAcceleration = _boostAcceleration;
        _currentMaxSpeed = _boostMaxSpeed;

        if (_boostRemaining > 0)
        {
            _isBoosting = true;
            _boostRemaining -= Time.deltaTime;
        }
        else
        {
            _boostRemaining = 0;
            StartCoroutine(BoostCooldown());
        }
    }


    private IEnumerator BoostCooldown()
    {
        _boostOnCooldown = true;
        _isBoosting = false;
        _boostRemaining = _boostDuration;
        yield return new WaitForSeconds(_boostCooldown);
        _boostOnCooldown = false;
    }

    private void Drift()
    {
        _isDrifting = _movementControls.Drift;
        _currentTurnSpeed = _movementControls.Drift ? _driftTurnSpeed : _standardTurnSpeed;

        if (_driftTrailsActive != _isDrifting)
        {
            _driftTrailsActive = _isDrifting;
            foreach (var trail in _driftTrails)
            {
                trail.emitting = _driftTrailsActive;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.GetComponent<Wall>()) return;
        var vel = _previousVel;
        vel.x = -vel.x;
        vel.y = 1;
        _rb.velocity = vel;
        var rot = Quaternion.Lerp(_rb.rotation, Quaternion.Euler(Vector3.forward), 0.5f);
        _rb.MoveRotation(rot);
        _rb.angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_centerOfMass, 0.02f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}