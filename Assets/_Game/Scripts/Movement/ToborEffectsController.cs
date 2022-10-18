using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborEffectsController : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private List<TrailRenderer> _driftTrails = new List<TrailRenderer>();
    [SerializeField] private List<TrailRenderer> _boostTrails = new List<TrailRenderer>();

    private MovementController _mc;
    private Animator _animator;
    private Rigidbody _rb;

    private bool _driftTrailsActive;
    private bool _boostTrailsActive;

    private bool _canPlayImpactAnim = true;

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

    public void PlayOnIdle()
    {
        _animator.SetBool("Idle", true);
    }

    public void PlayOnMoving()
    {
        _animator.SetBool("Idle", false);
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
