using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class ToborEffectsController : MonoBehaviour
{
    [Header("Trails")]
    [SerializeField] private List<TrailRenderer> _driftTrails = new List<TrailRenderer>();
    [SerializeField] private List<TrailRenderer> _boostTrails = new List<TrailRenderer>();

    [Header("Food Particles")]
    [SerializeField] private bool _enableFoodTrail = true;
    [SerializeField] private ParticleSystem _foodTrail;
    [SerializeField] private List<GameObject> _foodPool = new List<GameObject>();
    [Range(2,10)]
    [SerializeField] private float _velocityChangeRequired = 3f;

    [Header("Impact Stars")]
    [SerializeField] private bool _enableImpactStars = true;
    [SerializeField] private ParticleSystem _impactStars;

    [Header("Smoke")]
    [SerializeField] private bool _enableDriftSmoke = true;
    [SerializeField] private List<VisualEffect> _driftSmoke = new List<VisualEffect>();

    [Header("Landing")]
    [SerializeField] private ParticleSystem _landingPS;

    [SerializeField] private List<ParticleSystem> _boostBursts = new List<ParticleSystem>();

    private MovementController _mc;
    private Animator _animator;
    private Rigidbody _rb;

    private bool _driftTrailsActive;
    private bool _boostTrailsActive;

    private bool _canPlayImpactAnim = true;
    private bool _canFoodBurst = true;

    private float _storedVelocity;
    private bool _checkingVelocity = false;

    private bool _isGroundedCheck = false;
    private bool _isBoostingCheck = false;

    private void Start()
    {
        _mc = GetComponent<MovementController>();
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_mc.IsMoving && _rb.velocity.magnitude > 2.5f) PlayOnMoving();
        else PlayOnIdle();

        if (_mc.IsDrifting) PlayOnDrift();

        StartCoroutine(VelocityChange(0.5f));

        if (_mc.IsGrounded) VelocityCheck(_velocityChangeRequired);

        if (_isGroundedCheck != _mc.IsGrounded)
        {
            _isGroundedCheck = _mc.IsGrounded;
            if (_mc.IsGrounded) _landingPS.Play();
        }

        if (_isBoostingCheck != _mc.IsBoosting)
        {
            _isBoostingCheck = _mc.IsBoosting;
            if (_mc.IsBoosting)
            {
                foreach (var burst in _boostBursts)
                {
                    burst.Play();
                    _animator.SetTrigger("Boost");
                }
            }
        }

    }

    private void FixedUpdate()
    {
        TrailEffects();
    }

    private void TrailEffects()
    {
        if (_boostTrailsActive != _mc.IsBoosting)
        {
            _boostTrailsActive = _mc.IsBoosting;
            foreach (var trail in _boostTrails)
            {
                trail.emitting = _boostTrailsActive;
            }
        }

        if (_driftTrailsActive != _mc.IsDrifting)
        {
            _driftTrailsActive = _mc.IsDrifting;
            foreach (var trail in _driftTrails)
            {
                trail.emitting = _driftTrailsActive;
            }
        }
    }
    public IEnumerator VelocityChange(float wait)
    {

        if (!_checkingVelocity)
        {
            _checkingVelocity = true;
            _storedVelocity = _rb.velocity.magnitude;
            yield return new WaitForSeconds(wait);
            _checkingVelocity = false;
        }
    }

    public void VelocityCheck(float velocityMinimum)
    {
        var difference = _storedVelocity - _rb.velocity.magnitude;
        if (difference > velocityMinimum)
        {
            if (_canFoodBurst) StartCoroutine(FoodTrailBurstDelay());
            _storedVelocity = _rb.velocity.magnitude;
        }
    }

    public void PlayOnIdle()
    {
        _animator.SetBool("Idle", true);
    }

    public void PlayOnMoving()
    {
        _animator.SetBool("Idle", false);
    }

    public void PlayOnDrift()
    {
        if (_enableDriftSmoke)
        {
            foreach (var smoke in _driftSmoke)
            {
                smoke.Play();
            }
        }
    }

    public void SetFoodTrail(int count)
    {
        var em = _foodTrail.emission;
        em.rateOverDistance = count;
    }

    public void FoodTrailBurst(int count)
    { 
        //if (_enableFoodTrail) _foodTrail.Emit(_foodTrail.main.maxParticles);

        foreach (var food in _foodPool)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
            {
                for (var i = 0; i < count; i++)
                {
                    food.SetActive(true);
                    food.transform.position = _foodTrail.transform.position;
                    food.GetComponent<Rigidbody>().velocity = _rb.velocity;
                    food.GetComponent<Rigidbody>().angularVelocity = _rb.angularVelocity;
                }
                _storedVelocity = _rb.velocity.magnitude;
            }
        }
    }

    private IEnumerator FoodTrailBurstDelay()
    {
        _canFoodBurst = false;
        FoodTrailBurst(1);
        yield return new WaitForSecondsRealtime(3f);
        foreach (var food in _foodPool)
        {
            food.SetActive(false);
        }
        _canFoodBurst = true;
    }

    public void PlayOnCollision()
    {
        if (_canPlayImpactAnim)
        {
            if (_rb.velocity.magnitude > 10f)
            {
                _animator.SetFloat("ImpactMultiplier", 1f);
                _animator.SetTrigger("Impact");
            }
            else
            {
                _animator.SetFloat("ImpactMultiplier", 0.5f);
                _animator.SetTrigger("Impact");
            }
            StartCoroutine(LockImpactAnimation());
        }
        if (_enableImpactStars) _impactStars.Play();
    }

    public void PlayOnBouncePad()
    {

    }

    public void PlayOnCatapult()
    {

    }

    public void PlayOnBoostPad()
    {

    }

    private IEnumerator LockImpactAnimation()
    {
        _canPlayImpactAnim = false;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        _canPlayImpactAnim = true;
    }
}
