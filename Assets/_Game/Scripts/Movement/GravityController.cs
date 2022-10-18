using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] private bool _useCustomGravity = false;
    [Range(0,400)]
    [Tooltip("Stacks with rigidbody gravity. 0 is off, 400 is limit")]
    [SerializeField] private float _groundedGravity = 9.81f;
    //[Range(0,500)]
    //[SerializeField] private float _inAirGravity = 9.81f;

    [Header("Debug")]
    [SerializeField, ReadOnly] private float _currentGravity = 9.81f;

    public bool GravityEnabled { get; set; } = true;
    public bool UseCustomGravity => _useCustomGravity;
    public float Gravity => _currentGravity;

    private Rigidbody _rb;
    private MovementController _mc;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mc = GetComponent<MovementController>();

        _currentGravity = _groundedGravity;
    }

    private void FixedUpdate()
    {
        if (_useCustomGravity && GravityEnabled) _rb.AddForce(new Vector3(0,-_currentGravity, 0),ForceMode.Force);

        /*
        if (!_mc.IsGrounded) _currentGravity = _inAirGravity;
        else _currentGravity = _groundedGravity;
        */
    }
}
