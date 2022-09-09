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
    [SerializeField] private Transform _groundCheck = null;
    [SerializeField] private Vector3 _centerOfMass = Vector3.zero;

    [Header("Current Movement Stats")] 
    [SerializeField] private float _currentAcceleration = 0f;
    [SerializeField] private float _currentMaxSpeed = 0f;
    [SerializeField] private float _currentTurnSpeed = 0f;

    [Header("Standard Movement")]
    [SerializeField] private float _acceleration = 0f;
    [SerializeField] private float _maxSpeed = 0f;
    [SerializeField] private float _standardTurnSpeed = 0.75f;

    [Header("Drifting")]
    [SerializeField] private float _driftTurnSpeed = 0.25f;

    [Header("Boosting")] 
    [SerializeField] private float _boostAcceleration = 0f;
    [SerializeField] private float _boostMaxSpeed = 0f;
    [SerializeField] private float _boostDuration = 2f;
    [SerializeField] private float _boostCooldown = 2f;
    [SerializeField] private float _boostRemaining = 2f;
    [SerializeField] private bool _boostOnCooldown = false;

    private float _turnSmoothVel;
    private bool _isMoving = false;
    private Vector3 _direction;

    private Rigidbody _rb;
    private MovementControls _movementControls;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;

        _movementControls = GetComponent<MovementControls>();

        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
    }

    private void FixedUpdate()
    {
        if (!IsGrounded() || _isMoving) Steer();

        if (IsGrounded()) Movement();

    }

    private void Steer()
    {
        _direction = new Vector3(_movementControls.DirectionVector.x, 0f, _movementControls.DirectionVector.y);

        if (_direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(_rb.rotation.eulerAngles.y, targetAngle, ref _turnSmoothVel, _currentTurnSpeed);
            _rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }
    }

    private void Movement()
    {
        Vector3 forceVector = transform.forward * _currentAcceleration * _movementControls.Speed;

        if (_movementControls.Speed != 0)
        {
            if (_movementControls.Boost && !_boostOnCooldown)
            {
                Boost();
            }
            else Drive();
        }
        else
        {
            if (_movementControls.Boost && !_boostOnCooldown)
            {
                Boost();
            }
        }

        if (!_movementControls.Boost && _boostRemaining < _boostDuration)
        {
            StartCoroutine(BoostCooldown());
        }

        _rb.AddForce(transform.forward * _currentAcceleration * _movementControls.Speed, ForceMode.Acceleration);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _currentMaxSpeed);


        if (_rb.velocity == Vector3.zero)
        {
            _isMoving = false;
        }
        else _isMoving = true;

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
        _boostRemaining = _boostDuration;
        yield return new WaitForSeconds(_boostCooldown);
        _boostOnCooldown = false;
    }

    private void Drift()
    {
        if (_movementControls.Drift)
        {
            _currentTurnSpeed = _driftTurnSpeed;
        }
        else
        {
            _currentTurnSpeed = _standardTurnSpeed;
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Wall>())
        {
            _rb.AddForceAtPosition(collision.impulse/2,collision.GetContact(0).point,ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_centerOfMass, 0.25f);
    }
}
