using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck = null;
    [SerializeField] private Vector3 _centerOfMass = Vector3.zero;

    [Header("Standard Movement")]
    [SerializeField] private float _acceleration = 0f;
    [SerializeField] private float _maxSpeed = 0f;
    [SerializeField] private float _standardTurnSpeed = 0.5f;

    [Header("Drifting")]
    [SerializeField] private float _driftTurnSpeed = 0.1f;

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
    private float _currentTurnSpeed;

    private Rigidbody _rb;
    private MovementControls _movementControls;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;

        _movementControls = GetComponent<MovementControls>();
    }

    private void FixedUpdate()
    {
        if (!IsGrounded() || _isMoving) Steer();

        Drive();

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

    private void Drive()
    {

        if (_movementControls._movementInput.Player.Drive.IsPressed())
        {
            _rb.AddForce(transform.forward * (_acceleration * _movementControls.Speed), ForceMode.Acceleration);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);

            if (_boostRemaining != _boostDuration) StartCoroutine(BoostCooldown());

        }

        if (_movementControls.Boost && !_boostOnCooldown)
        {
            _rb.AddForce(transform.forward * _boostAcceleration, ForceMode.Acceleration);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _boostMaxSpeed);

            BoostTimer();
        }

        if (_rb.velocity == Vector3.zero)
        {
            _isMoving = false;
        }
        else _isMoving = true;

        Drift();

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

    private IEnumerator BoostCooldown()
    {
        _boostOnCooldown = true;
        _boostRemaining = _boostDuration;
        yield return new WaitForSeconds(_boostCooldown);
        _boostOnCooldown = false;
    }


    private void BoostTimer()
    {
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

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_centerOfMass, 0.5f);
    }
}
