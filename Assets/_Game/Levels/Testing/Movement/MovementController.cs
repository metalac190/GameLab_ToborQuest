using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Vector3 _centerOfMass = Vector3.zero;

    [Header("Grounded")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Turtled")] 
    [SerializeField] private Transform _roofCheck;
    [SerializeField] private float _roofCheckRadius = 0.5f;

    [Header("Wall Bounce")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _upwardBounce;

    [Header("Movement")]
    [SerializeField] private float _acceleration = 20;
    [SerializeField] private float _maxSpeed = 30;
    [Tooltip("Don't Even Think About Messing With This Variable")]
    //Seriously don't touch this
    //Default 2.09, 1.87, 1.698
    [SerializeField, ReadOnly] private Vector3 _customInertiaTensor = new Vector3(2.09f, 20f, 5f);

    [Header("Turning")]
    [SerializeField] private bool _turnWhenStopped = true;
    [SerializeField] private float _stoppedTurnSpeed = 2f;
    [SerializeField] private float _standardTurnSpeed = 3f;

    [Header("Side Flip")]
    [SerializeField] private float _flipLaunch = 5f;
    [SerializeField] private float _flipLaunchTorque = 5f;
    [SerializeField] private float _sideFlipAngularDrag = 2f;

    [Header("Drifting")]
    [SerializeField] private float _driftTurnSpeed = 5.0f;
    [SerializeField] private List<TrailRenderer> _driftTrails = new List<TrailRenderer>();

    [Header("Boosting")] 
    [SerializeField] private float _boostAcceleration = 30;
    [SerializeField] private float _boostMaxSpeed = 60;
    [SerializeField] private float _boostDuration = 2f;
    [SerializeField] private float _boostCooldown = 2f;
    [SerializeField] private float _boostRemaining = 2f;
    [SerializeField] private bool _boostOnCooldown = false;
    [SerializeField] private List<TrailRenderer> _boostTrails = new List<TrailRenderer>();

    [Header("Debug")]
    [SerializeField, ReadOnly] private float _currentAcceleration = 0f;
    [SerializeField, ReadOnly] private float _currentMaxSpeed = 0f;
    [SerializeField, ReadOnly] private float _currentTurnSpeed = 0f;
    [SerializeField, ReadOnly] private float _currentMaxAngularVelocity = 0f;
    //[SerializeField, ReadOnly] private float _turnSmoothVel;
    [SerializeField, ReadOnly] private bool _isMoving;
    [SerializeField, ReadOnly] private bool _isGrounded;
    [SerializeField, ReadOnly] private bool _isTurtled;
    [SerializeField, ReadOnly] private bool _isDrifting;
    [SerializeField, ReadOnly] private bool _isBoosting;
    [SerializeField, ReadOnly] private bool _isFlipping;
    [SerializeField, ReadOnly] private Vector3 _direction;
    [SerializeField, ReadOnly] private Vector3 _previousVel;

    private Rigidbody _rb;
    private MovementControls _movementControls;
    private bool _driftTrailsActive;
    private bool _boostTrailsActive;

    private bool IsGrounded() => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
    private bool IsTurtled() => Physics.CheckSphere(_roofCheck.position, _roofCheckRadius, _groundLayer);

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;

        _movementControls = GetComponent<MovementControls>();

        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
        _currentTurnSpeed = _stoppedTurnSpeed;

        _rb.inertiaTensor = _customInertiaTensor;
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        _isTurtled = IsTurtled();

        if ((_isGrounded || _turnWhenStopped) && !_isFlipping) Steer();

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
        _currentMaxAngularVelocity = _rb.maxAngularVelocity;
    }

    private void Steer()
    {
        _direction = new Vector3(_movementControls.DirectionVector.x, 0f, _movementControls.DirectionVector.y);

        if (_direction.magnitude >= 0.1f)
        {

            /*
            var currentAngle = _rb.rotation.eulerAngles.y;
            var targetAngle = currentAngle + Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref _turnSmoothVel, _currentTurnSpeed);
            _rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
            */

            var torque = Mathf.LerpAngle(_direction.x,_direction.z,Time.fixedDeltaTime) * _rb.transform.up * 1000 * Time.fixedDeltaTime;
            _rb.maxAngularVelocity = _currentTurnSpeed;
            _rb.AddRelativeTorque(torque,ForceMode.VelocityChange);

        }
    }

    private void Movement()
    {
        if (_movementControls.Boost && !_boostOnCooldown) Boost();
        else if (_movementControls.Speed != 0) Drive();

        if (!_movementControls.Boost && _boostRemaining < _boostDuration) StartCoroutine(BoostCooldown());

        _rb.AddForce(transform.forward * (_currentAcceleration * _movementControls.Speed), ForceMode.Acceleration);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _currentMaxSpeed);

        _isMoving = _rb.velocity.magnitude > 0.5f;

        Drift();
        SideFlip();

    }

    private void Drive()
    {
        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
    }

    private void Boost()
    {

        if (_movementControls.Speed <= 0) return;

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

    private void SideFlip()
    {
        if (_movementControls.SideFlip != 0) StartCoroutine(FlipForces(_movementControls.SideFlip));
    }

    private IEnumerator FlipForces(float direction)
    {
        _isFlipping = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.maxAngularVelocity = float.PositiveInfinity;
        var tempDrag = _rb.angularDrag;

        _rb.AddRelativeForce(new Vector3(direction*_flipLaunch, _flipLaunch, 0f), ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.05f);

        _rb.angularDrag = _sideFlipAngularDrag;
        _rb.AddRelativeTorque(new Vector3(0f, 0f, -direction*_flipLaunchTorque), ForceMode.VelocityChange);

        yield return new WaitUntil(IsGrounded);
        _rb.angularDrag = tempDrag;
        _isFlipping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ExaggeratedWallBounce(collision);
    }



    //Bounce with direct momentum transfer
    private void StandardWallBounce(Collision collision)
    {
        if (collision.gameObject.layer != _wallLayer) return;
        var vel = _previousVel;
        vel.x = -vel.x;
        vel.y = 1;
        _rb.velocity = vel;
        var rot = Quaternion.Lerp(_rb.rotation, Quaternion.Euler(Vector3.forward), 0.5f);
        _rb.MoveRotation(rot);
        _rb.angularVelocity = Vector3.zero;
    }

    // Wall Bounce with exaggerated momentum transfer
    private void ExaggeratedWallBounce(Collision collision)
    {
        if ((_wallLayer.value & (1 << collision.gameObject.layer)) <= 0) return;
        _rb.AddForce(new Vector3(0, _upwardBounce, 0), ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_centerOfMass, 0.02f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_roofCheck.position, _roofCheckRadius);
    }
}
