using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _object;
    [SerializeField] private bool _x;
    [SerializeField] private bool _y;
    [SerializeField] private bool _z;

    [SerializeField] private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 pos = _startPos;
        if (_x) pos.x = _object.position.x;
        if (_y) pos.y = _object.position.y;
        if (_z) pos.z = _object.position.z;
        transform.position = pos;
    }
}
