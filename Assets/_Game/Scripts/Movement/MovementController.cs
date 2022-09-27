using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MovementController : MonoBehaviour
{
    [Header("Ridgidbody Stats")]
    [SerializeField] private Vector3 _centerOfMass = Vector3.zero;
    [Tooltip("Don't Even Think About Messing With This Variable")]
    //Seriously don't touch this
    //Default 2.09, 1.87, 1.698
    [SerializeField, ReadOnly] private Vector3 _customInertiaTensor = new Vector3(2.09f, 20f, 5f);

    [Header("Position Check")] [SerializeField] private bool _showPositionCheckTab;
    [SerializeField, ShowIf("_showPositionCheckTab")] private Transform _groundCheck;
    [SerializeField, ShowIf("_showPositionCheckTab")] private float _groundCheckRadius = 0.2f;
    [SerializeField, ShowIf("_showPositionCheckTab")] private LayerMask _groundLayer;
    [SerializeField, ShowIf("_showPositionCheckTab")] private Transform _roofCheck;
    [SerializeField, ShowIf("_showPositionCheckTab")] private float _roofCheckRadius = 0.5f;

    [Header("Movement")] [SerializeField] private bool _showMovementTab;
    [SerializeField, ShowIf("_showMovementTab")] private float _acceleration = 20;
    [SerializeField, ShowIf("_showMovementTab")] private float _maxSpeed = 30;

    [Header("Steering")] [SerializeField] private bool _showSteeringTab;
    [SerializeField, ShowIf("_showSteeringTab")] private bool _turnWhenStopped = true;
    [SerializeField, ShowIf("_showSteeringTab")] private float _stoppedTurnSpeed = 2f;
    [SerializeField, ShowIf("_showSteeringTab")] private float _standardTurnSpeed = 3f;

    [Header("Side Flip")] [SerializeField] private bool _showSideFlipTab;
    [SerializeField, ShowIf("_showSideFlipTab")] private Vector2 _flipLaunch;
    [SerializeField, ShowIf("_showSideFlipTab")] private float _flipLaunchTorque = 10f;
    [SerializeField, ShowIf("_showSideFlipTab")] private float _sideFlipAngularDrag = 2f;

    [Header("Drift")] [SerializeField] private bool _showDrifitingTab;
    [SerializeField, ShowIf("_showDrifitingTab")] private float _driftTurnSpeed = 5.0f;

    [Header("Boost")] [SerializeField] private bool _showBoostTab;
    [SerializeField, ShowIf("_showBoostTab")] private bool _canBoostInAir;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostAcceleration = 30;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostMaxSpeed = 60;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostDuration = 2f;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostCooldown = 2f;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostRemaining = 2f;
    [SerializeField, ShowIf("_showBoostTab")] private bool _boostOnCooldown = false;

    [Header("Effects")] [SerializeField] private bool _showEffectsTab;
    [SerializeField, ShowIf("_showEffectsTab")] private List<TrailRenderer> _driftTrails = new List<TrailRenderer>();
    [SerializeField, ShowIf("_showEffectsTab")] private List<TrailRenderer> _boostTrails = new List<TrailRenderer>();

    [Header("Debug")]
    [SerializeField, ReadOnly] private float _currentAcceleration = 0f;
    [SerializeField, ReadOnly] private float _velocityMagnitude;
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
    [SerializeField, ReadOnly] public bool _UsingPad;
    [SerializeField, ReadOnly] private Vector3 _direction;
    [SerializeField, ReadOnly] private Vector3 _previousVel;

    public bool IsMoving => _isMoving;
    public bool IsGrounded => _isGrounded;
    public bool IsTurtled => _isTurtled;
    public bool IsDrifting => _isDrifting;
    public bool IsBoosting => _isBoosting;
    public bool IsFlipping => _isFlipping;

    private Rigidbody _rb;
    private MovementControls _movementControls;
    private bool _driftTrailsActive;
    private bool _boostTrailsActive;

    protected bool GroundCheck() => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
    protected bool TurtledCheck() => Physics.CheckSphere(_roofCheck.position, _roofCheckRadius, _groundLayer);

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
        _isGrounded = GroundCheck();
        _isTurtled = TurtledCheck();

        if ((_isGrounded || _turnWhenStopped) && !_isFlipping) Steer();

        if (_movementControls.Boost && !_boostOnCooldown) Boost();

        if (_isGrounded || _isBoosting) Movement();


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
        _velocityMagnitude = _rb.velocity.magnitude;
    }

    private void Steer()
    {
        _direction = new Vector3(_movementControls.DirectionVector.x, 0f, _movementControls.DirectionVector.y);

        if (_direction.magnitude >= 0.1f)
        {
            var torque = Mathf.LerpAngle(_direction.x,_direction.z,Time.fixedDeltaTime) * _rb.transform.up * 1000 * Time.fixedDeltaTime;
            _rb.maxAngularVelocity = _currentTurnSpeed;
            _rb.AddRelativeTorque(torque,ForceMode.VelocityChange);

        }
    }

    private void Movement()
    {
        if (_movementControls.Speed != 0 && !_isBoosting) Drive();

        if (!_movementControls.Boost && _boostRemaining < _boostDuration) StartCoroutine(BoostCooldown());

        var force = transform.forward * (_currentAcceleration * _movementControls.Speed);

        if (_isBoosting) force.y = 0;

        _rb.AddForce(force, ForceMode.Acceleration);
        
        if(!_UsingPad) _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _currentMaxSpeed);
        
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _currentMaxSpeed);
        
        _isMoving = _rb.velocity.magnitude > 0.25f;

        if (_isGrounded) Drift();

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
            StartCoroutine(BoostCooldown());
        }
    }


    public IEnumerator BoostCooldown(float cooldown = 0)
    {
        _boostOnCooldown = true;
        _boostRemaining = 0;
        _isBoosting = false;
        _boostRemaining = _boostDuration;

        if (cooldown == 0) yield return new WaitForSeconds(_boostCooldown);
        else yield return new WaitForSeconds(cooldown);

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

        _rb.AddRelativeForce(new Vector3(direction*_flipLaunch.x, _flipLaunch.y, 0f), ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.05f);

        _rb.angularDrag = _sideFlipAngularDrag;
        _rb.AddRelativeTorque(new Vector3(0f, 0f, -direction*_flipLaunchTorque), ForceMode.VelocityChange);

        yield return new WaitUntil(GroundCheck);
        _rb.angularDrag = tempDrag;
        _isFlipping = false;
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_centerOfMass, 0.02f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_roofCheck.position, _roofCheckRadius);
    }
    */
    
}
