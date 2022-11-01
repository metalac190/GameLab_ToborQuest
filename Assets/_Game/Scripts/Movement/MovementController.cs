using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

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
    //[SerializeField, ShowIf("_showSteeringTab")] private bool _turnWhenStopped = true;
    [SerializeField, ShowIf("_showSteeringTab")] private float _stoppedTurnSpeed = 2f;
    [SerializeField, ShowIf("_showSteeringTab")] private float _standardTurnSpeed = 3f;

    [Header("Side Flip")] [SerializeField] private bool _showSideFlipTab;
    [SerializeField, ShowIf("_showSideFlipTab")] private Vector2 _flipLaunch;
    [SerializeField, ShowIf("_showSideFlipTab")] private float _flipLaunchTorque = 10f;
    [SerializeField, ShowIf("_showSideFlipTab")] private float _sideFlipAngularDrag = 2f;
    [SerializeField, ShowIf("_showSideFlipTab")] private bool _cancelMomentumBeforeFlip = false;

    [Header("Drift")] [SerializeField] private bool _showDrifitingTab;
    [Range(0,1), ShowIf("_showDrifitingTab")]
    [SerializeField] private float _driftAccelerationMultiplier = 0.5f;
    [SerializeField, ShowIf("_showDrifitingTab")] private float _driftTurnSpeed = 5.0f;
    [SerializeField, ShowIf("_showDrifitingTab")] private float _driftWheelDampeningRate = 2.0f;
    [SerializeField, ShowIf("_showDrifitingTab")] private float _driftFrictionStiffness = 0.25f;

    [Header("Boost")] [SerializeField] private bool _showBoostTab;
    [SerializeField, ShowIf("_showBoostTab")] private bool _canBoostInAir;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostAcceleration = 30;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostMaxSpeed = 60;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostDuration = 2f;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostCooldown = 2f;
    [SerializeField, ShowIf("_showBoostTab")] private float _boostTimeRemaining = 2f;
    [SerializeField, ShowIf("_showBoostTab")] private bool _useRechargeBoost = false;
    [SerializeField, ShowIf("_showBoostTab")] private float _rechargeDelay = 0.5f;

    [Header("Debug")]
    [SerializeField, ReadOnly] private float _currentAcceleration = 0f;
    [SerializeField, ReadOnly] private float _velocityMagnitude;
    [SerializeField, ReadOnly] private float _currentMaxSpeed = 0f;
    [SerializeField, ReadOnly] private float _currentTurnSpeed = 0f;
    [SerializeField, ReadOnly] private float _currentMaxAngularVelocity = 0f;
    [SerializeField, ReadOnly] private bool _boostOnCooldown = false;
    //[SerializeField, ReadOnly] private float _turnSmoothVel;
    [SerializeField, ReadOnly] private bool _isMoving;
    [SerializeField, ReadOnly] private bool _isGrounded;
    [SerializeField, ReadOnly] private bool _isTurtled;
    [SerializeField, ReadOnly] private bool _isDrifting;
    [SerializeField, ReadOnly] private bool _isBoosting;
    [SerializeField, ReadOnly] private bool _isFlipping;
    [SerializeField, ReadOnly] private Vector3 _direction;
    [SerializeField, ReadOnly] private Vector3 _previousVel;

    public bool IsMoving => _isMoving;
    public bool IsGrounded => _isGrounded;
    public bool IsTurtled => _isTurtled;
    public bool IsDrifting => _isDrifting;
    public bool IsBoosting => _isBoosting;
    public bool IsFlipping => _isFlipping;
    public bool UsingRechargeBoost => _useRechargeBoost;
    public Vector3 PreviousVelocity => _previousVel;

    private float _inputSpeed;

    private Rigidbody _rb;
    private MovementControls _movementControls;
    private ToborEffectsController _ec;
    private WheelsController _wc;

    //public bool GroundCheck() => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
    public bool GroundCheck() => _wc.WheelsGroundCheck();
    protected bool TurtledCheck() => Physics.CheckSphere(_roofCheck.position, _roofCheckRadius, _groundLayer);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _movementControls = GetComponent<MovementControls>();
        _wc = GetComponent<WheelsController>();
        _ec = GetComponent<ToborEffectsController>();
    }

    private void Start()
    {

        _rb.centerOfMass = _centerOfMass;
        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
        _currentTurnSpeed = _stoppedTurnSpeed;
        _rb.inertiaTensor = _customInertiaTensor;
    }

    private void FixedUpdate()
    {
        _isGrounded = GroundCheck();
        _isTurtled = TurtledCheck();

        if (!_isFlipping) Steer();

        if (_movementControls.Boost && !_boostOnCooldown) Boost();

        if (_isGrounded || _isBoosting) Movement();

        if (_isGrounded) SideFlip();

        Drift();
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

            // Max Angular Velocity set
            _rb.maxAngularVelocity = _currentTurnSpeed;
            _rb.AddRelativeTorque(torque,ForceMode.VelocityChange);
        }
    }

    private void Movement()
    {
        //if speed is not 0 AND not boosting
        //tobor can drive
        if (_movementControls.Speed != 0 && !_isBoosting) Drive();

        //if tobor is not boosting AND boost has not been fully used
        //start boost cooldown
        if (!_movementControls.Boost && _boostTimeRemaining < _boostDuration)
        {
            if (_useRechargeBoost) StartCoroutine(BoostRecharge());
            else StartCoroutine(BoostCooldown());
        }

        var force = new Vector3();
        if (_isBoosting) _inputSpeed = 1f;
        else _inputSpeed = _movementControls.Speed;

        if (_isDrifting) force = transform.forward * (_currentAcceleration * Mathf.Clamp(_inputSpeed, -_driftAccelerationMultiplier, _driftAccelerationMultiplier));
        else force = transform.forward * (_currentAcceleration * _inputSpeed);

        //boosting gives no y velocity
        if (_isBoosting) force.y = 0;

        _rb.AddForce(force, ForceMode.Acceleration);

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _currentMaxSpeed);
        
        _isMoving = _rb.velocity.magnitude > 0.25f;
    }

    private void Drive()
    {
        _currentAcceleration = _acceleration;
        _currentMaxSpeed = _maxSpeed;
    }

    private void Boost()
    {
        //if (_movementControls.Speed <= 0) return;
        
        _currentAcceleration = _boostAcceleration;
        _currentMaxSpeed = _boostMaxSpeed;

        if (_boostTimeRemaining > 0)
        {
            _isBoosting = true;
            _boostTimeRemaining -= Time.deltaTime;
        }
        else
        {
            if (_useRechargeBoost) StartCoroutine(BoostRecharge());
            else StartCoroutine(BoostCooldown());
        }

    }


    public IEnumerator BoostCooldown(float cooldown = 0)
    {
        _boostOnCooldown = true;
        _boostTimeRemaining = 0;
        _isBoosting = false;
        _boostTimeRemaining = _boostDuration;

        if (cooldown == 0) yield return new WaitForSeconds(_boostCooldown);
        else yield return new WaitForSeconds(cooldown);

        _boostOnCooldown = false;
    }

    public IEnumerator BoostRecharge()
    {
        _boostOnCooldown = true;
        _isBoosting = false;
        yield return new WaitForSeconds(_rechargeDelay);
        _boostOnCooldown = false;

        _boostTimeRemaining += Time.deltaTime;
        _boostTimeRemaining = Mathf.Clamp(_boostTimeRemaining, 0f, _boostDuration);

        if (_isBoosting) yield break;
        yield return new WaitUntil(() => _boostTimeRemaining >= _boostDuration);
    }

    public float BoostPercentage()
    {
        return (_boostTimeRemaining / _boostDuration);
    }

    private void Drift()
    {
        if (_isGrounded)
        {
            _isDrifting = _movementControls.Drift;

            if (_movementControls.Drift)
            {
                _currentTurnSpeed = _driftTurnSpeed;
                //_wc.SetWheelFriction(_driftWheelDampeningRate, _driftFrictionStiffness);
                StartCoroutine(DriftFriction());
            }
            else
            {
                _currentTurnSpeed = _standardTurnSpeed;
                //_wc.SetWheelFriction(_wc.StandardDampeningRate, _wc.StandardFrictionStiffness);
            }
        }
        else
        {
            _isDrifting = false;
        }
    }

    private IEnumerator DriftFriction()
    {
        _wc.SetWheelFriction(_driftWheelDampeningRate, _driftFrictionStiffness);
        yield return new WaitUntil(() => !_isDrifting);
        _wc.SetWheelFriction(_wc.StandardDampeningRate,_wc.StandardFrictionStiffness);
    }

    private void SideFlip()
    {
        if (_movementControls.SideFlip != 0) StartCoroutine(FlipForces(_movementControls.SideFlip));
    }

    private IEnumerator FlipForces(float direction)
    {
        _isFlipping = true;
        if (!_cancelMomentumBeforeFlip)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

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

    public void SetActive(bool state)
    {
        if (!state)
        {
            _isDrifting = false;
            _isFlipping = true;

            if (_useRechargeBoost) StartCoroutine(BoostRecharge());
            else StartCoroutine(BoostCooldown(_boostCooldown));
            this.enabled = false;
        }
        else
        {
            _isFlipping = false;
            this.enabled = true;
        }
    }



    /*
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position + _centerOfMass,_groundCheckRadius);
    }
    */
}
