using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Header("Wheel")]
    [SerializeField] private float _mass;
    [SerializeField] private float _radius;
    [SerializeField, ReadOnly] private float _inertia;
    [SerializeField] private Vector3 _center;

    [Header("Suspension")]
    [Range(0,1)]
    [SerializeField] private float _distance = 0.3f;
    [Range(0, 1)]
    [SerializeField] private float _ancor;
    [SerializeField] private float _springRate = 501.4f;
    [SerializeField] private float _damperRate = 25.07f;
    [SerializeField, ReadOnly] private float _maxForce;
    [SerializeField] private float _targetPosition = 0.5f;

    [Header("Steering")]
    [SerializeField] private float _maxAngle = 90f;
    [SerializeField] private float _offset = 0f;

    [Header("Friction")]
    [SerializeField] private float _wheelDampeningRate = 0.25f;
    [SerializeField] private float _frictionStiffness = 1f;

    [Header("Objects")]
    [SerializeField] public WheelCollider _wheelCollider;
    [SerializeField] private Transform _wheelMesh;


    private Rigidbody _rb;
    private float _turnAngle;

    private void Start()
    {
        if (!_wheelCollider) _wheelCollider = GetComponentInChildren<WheelCollider>();
        _rb = GetComponentInParent<Rigidbody>();
        UpdateSuspension();
    }

    private void Update()
    {
        UpdateSuspension();
    }

    private void UpdateSuspension()
    {
        //inertia = (1/2) * m * r^2
        _inertia = (1 / 2) * _mass * Mathf.Pow(_radius, 2);
        //spring rate = vehicle mass / num wheels * 2 * 9.81 / suspension distance
        _springRate = _rb.mass / 6 * 2 * 9.81f / _distance;
        //Damper rate = spring rate / 20
        _damperRate = _springRate / 20;

        _wheelCollider.mass = _mass;
        _wheelCollider.center = _center;
        _wheelCollider.suspensionDistance = _distance;

        var springJoint = _wheelCollider.suspensionSpring;
        springJoint.spring = _springRate;
        springJoint.damper = _damperRate;
        springJoint.targetPosition = _targetPosition;
        _wheelCollider.suspensionSpring = springJoint;

        _wheelCollider.wheelDampingRate = _wheelDampeningRate;

        var ffrictionCurve = _wheelCollider.forwardFriction;
        ffrictionCurve.stiffness = _frictionStiffness;
        _wheelCollider.forwardFriction = ffrictionCurve;

        var sfrictionCurve = _wheelCollider.sidewaysFriction;
        sfrictionCurve.stiffness = _frictionStiffness;
        _wheelCollider.sidewaysFriction = sfrictionCurve;
    }

    public void Steer(float steerInput)
    {
        _turnAngle = steerInput * _maxAngle + _offset;
        _wheelCollider.steerAngle = _turnAngle;
    }

    public void UpdatePosition()
    {
        var pos = transform.position;
        var rot = transform.rotation;

        _wheelCollider.GetWorldPose(out pos, out rot);
        _wheelMesh.transform.position = pos;
        _wheelMesh.transform.rotation = rot;
    }
}