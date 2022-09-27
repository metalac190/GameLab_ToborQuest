using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private List<Wheel> _wheels = new List<Wheel>();

    /*
    [Header("Wheel Properties")]
    [Header("Friction")]
    [Tooltip("How resistive the ground forces are")]
    [SerializeField] private float _wheelDampeningRate = 0.25f;
    [Tooltip("How sticky/slick the current surface is")]
    [SerializeField] private float _frictionStiffness = 1f;
    [Header("Suspension")]
    [Tooltip("Travel distance of the suspension")]
    [SerializeField] private float _suspensionDistance = 0.3f;
    [Tooltip("How fast the suspension reaches a position, Larger value means faster")]
    [SerializeField] private float _suspensionSpring = 2000f;
    [Tooltip("Dampens suspension speed, Larger value makes spring move slower")]
    [SerializeField] private float _suspensionDamper = 45f;
    [SerializeField] private float _targetPosition = 0.5f;
    */
    [Header("Collisions")]
    [SerializeField] private LayerMask _wallLayer;

    [Header("Debug")]
    [SerializeField, ReadOnly] private bool _canCollide = true;


    private MovementController _mc;
    private MovementControls _input;
    private Rigidbody _rb;

    private void Start()
    {
        _mc = GetComponent<MovementController>();
        _input = GetComponent<MovementControls>();
        _rb = GetComponent<Rigidbody>();

        _canCollide = true;
    }

    private void FixedUpdate()
    {
        UpdateWheels();
        //UpdateSuspension();
    }

    private void LateUpdate()
    {
        if (_canCollide) GetCollisions();
    }

    /*
    private void UpdateSuspension()
    {
        foreach (var w in _wheels)
        {
            w._wheelCollider.wheelDampingRate = _wheelDampeningRate;

            var ffrictionCurve = w._wheelCollider.forwardFriction;
            ffrictionCurve.stiffness = _frictionStiffness;
            w._wheelCollider.forwardFriction = ffrictionCurve;

            var sfrictionCurve = w._wheelCollider.sidewaysFriction;
            sfrictionCurve.stiffness = _frictionStiffness;
            w._wheelCollider.sidewaysFriction = sfrictionCurve;

            w._wheelCollider.suspensionDistance = _suspensionDistance;

            var springJoint = w._wheelCollider.suspensionSpring;
            springJoint.spring = _suspensionSpring;
            springJoint.damper = _suspensionDamper;
            springJoint.targetPosition = _targetPosition;
            w._wheelCollider.suspensionSpring = springJoint;
        }
    }
    */

    private void UpdateWheels()
    {

        foreach (var w in _wheels)
        {
            w.Steer(_input.DirectionVector.x);
            w.UpdatePosition();
        }
    }

    private void GetCollisions()
    {
        foreach (var w in _wheels)
        {
            if (w._wheelCollider.GetGroundHit(out var wheelHit))
            {
                if ((_wallLayer.value & (1 << wheelHit.collider.gameObject.layer)) <= 0) return;

                //Debug.Log("Wheel " + w.gameObject.name + ": " + wheelHit.collider.name);
                _mc.ExaggeratedWallBounce(wheelHit.collider, wheelHit.normal);
                StartCoroutine(CollisionCooldown());
            }

        }
    }

    private IEnumerator CollisionCooldown()
    {
        _canCollide = false;
        yield return new WaitForSeconds(0.1f);
        _canCollide = true;
    }
}
