using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private List<Wheel> _wheels = new List<Wheel>();

    [Header("Wheel Properties")]
    [Header("Friction")]
    [Tooltip("How resistive the ground forces are")]
    [SerializeField] private float _newWheelDampeningRate = 0.25f;
    [Tooltip("How sticky/slick the current surface is")]
    [SerializeField] private float _newFrictionStiffness = 1f;
    [Header("Suspension")]
    [Tooltip("Travel distance of the suspension")]
    [SerializeField] private float _newSuspensionDistance = 0.3f;
    [Tooltip("How fast the suspension reaches a position, Larger value means faster")]
    [SerializeField] private float _newSuspensionSpring = 2000f;
    [Tooltip("Dampens suspension speed, Larger value makes spring move slower")]
    [SerializeField] private float _newSuspensionDamper = 45f;


    private MovementController _mc;
    private MovementControls _input;
    private Rigidbody _rb;

    private void Start()
    {
        _mc = GetComponent<MovementController>();
        _input = GetComponent<MovementControls>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateWheels();
        UpdateSuspension();
    }

    private void UpdateSuspension()
    {
        foreach (var w in _wheels)
        {
            w._wheelCollider.wheelDampingRate = _newWheelDampeningRate;

            var ffrictionCurve = w._wheelCollider.forwardFriction;
            ffrictionCurve.stiffness = _newFrictionStiffness;
            w._wheelCollider.forwardFriction = ffrictionCurve;

            var sfrictionCurve = w._wheelCollider.sidewaysFriction;
            sfrictionCurve.stiffness = _newFrictionStiffness;
            w._wheelCollider.sidewaysFriction = sfrictionCurve;

            w._wheelCollider.suspensionDistance = _newSuspensionDistance;

            var springJoint = w._wheelCollider.suspensionSpring;
            springJoint.spring = _newSuspensionSpring;
            springJoint.damper = _newSuspensionDamper;
            w._wheelCollider.suspensionSpring = springJoint;
        }
    }

    private void UpdateWheels()
    {

        foreach (var w in _wheels)
        {
            w.Steer(_input.DirectionVector.x);
            w.UpdatePosition();
        }
    }
}
