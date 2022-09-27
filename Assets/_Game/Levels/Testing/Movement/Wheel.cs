using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _maxAngle = 90f;
    [SerializeField] private float _offset = 0f;

    [Header("Objects")]
    [SerializeField] public WheelCollider _wheelCollider;
    [SerializeField] private Transform _wheelMesh;

    [Header("Debug")]
    [SerializeField] private bool _printStats;

    private float _turnAngle;

    private void Start()
    {
        if (!_wheelCollider) _wheelCollider = GetComponentInChildren<WheelCollider>();
    }

    private void Update()
    {
        if (_printStats)
        {
            var pos = transform.position;
            var rot = transform.rotation;
            _wheelCollider.GetWorldPose(out pos, out rot);
            Debug.Log("Pos: " + pos + "Rot: " + rot);
            
        }
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
