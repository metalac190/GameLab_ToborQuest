using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborEffectsController : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private List<TrailRenderer> _driftTrails = new List<TrailRenderer>();
    [SerializeField] private List<TrailRenderer> _boostTrails = new List<TrailRenderer>();

    private MovementController _mc;
    private bool _driftTrailsActive;
    private bool _boostTrailsActive;

    private void Start()
    {
        _mc = GetComponent<MovementController>();
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
}
