using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectAlongAxis : MonoBehaviour
{
    [SerializeField] private bool _lateUpdate;
    [SerializeField] private bool _maintainOriginalOffset;
    [SerializeField] private Transform _object;
    [SerializeField] private bool _x;
    [SerializeField] private bool _y;
    [SerializeField] private bool _z;

    [SerializeField, ReadOnly] private Vector3 _startPos;
    [SerializeField, ReadOnly] private Vector3 _offset;

    private void Start()
    {
        _startPos = transform.position;
        _offset = _maintainOriginalOffset ? _startPos - _object.position : Vector3.zero;
    }
    private void OnEnable()
    {
        CameraAngleController.BeforeCameraMove += BeforeCameraMove;
    }
    
    private void OnDisable()
    {
        CameraAngleController.BeforeCameraMove -= BeforeCameraMove;
    }

    private void LateUpdate()
    {
        if (_lateUpdate) BeforeCameraMove();
    }

    private void BeforeCameraMove()
    {
        Vector3 pos = _startPos;
        if (_x) pos.x = _object.position.x + _offset.x;
        if (_y) pos.y = _object.position.y + _offset.y;
        if (_z) pos.z = _object.position.z + _offset.z;
        transform.position = pos;
    }
}
