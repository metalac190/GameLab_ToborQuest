using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] private bool _useCustomGravity = false;
    [Range(0,500)]
    [Tooltip("Stacks with rigidbody gravity. 0 is off, 500 is limit")]
    [SerializeField] private float _gravity = 9.81f;

    public bool UseCustomGravity => _useCustomGravity;
    public float Gravity => _gravity;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_useCustomGravity) _rb.AddForce(new Vector3(0,-_gravity, 0),ForceMode.Force);
    }
}
