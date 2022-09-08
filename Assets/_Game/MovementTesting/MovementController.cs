using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Vector3 _centerOfMass = Vector3.zero;
    [SerializeField] private float _acceleration = 0f;
    [SerializeField] private float _maxSpeed = 0f;

    private float _turnSmoothVel;
    private Vector3 _direction;

    private Rigidbody _rb;
    private Inputs _inputs;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;

        _inputs = GetComponent<Inputs>();
    }

    private void FixedUpdate()
    {
        Steer();

        if (_inputs.Speed > 0f) Drive();
    }

    private void Steer()
    {
        _direction = new Vector3(_inputs.DirectionVector.x, 0f, _inputs.DirectionVector.y);

        if (_direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(_rb.rotation.eulerAngles.y, targetAngle, ref _turnSmoothVel, 0.1f);
            _rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }
    }

    private void Drive()
    {
        _rb.AddForce(_direction * (_acceleration * _inputs.Speed), ForceMode.Acceleration);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
    }

    private void Drift()
    {
        if (_inputs.Drift)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_centerOfMass, 0.5f);
    }
}
