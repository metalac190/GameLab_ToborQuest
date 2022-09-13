using System;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _objToFollow;

    [SerializeField] private bool _move;
    [SerializeField, ShowIf("_move")] private float _moveSpeed = 10;
    [SerializeField, ShowIf("_move")] private bool _maintainOriginalOffset;
    [SerializeField, ShowIf("_move")] private Vector3 _localPositionOffset = Vector3.zero;
    [SerializeField, ShowIf("_move")] private Vector3 _worldPositionOffset = Vector3.zero;
    [SerializeField] private bool _rotate;
    [SerializeField, ShowIf("_rotate")] private float _rotateSpeed = 10;
    [SerializeField, ShowIf("_rotate")] private Vector3 _rotationOffset = Vector3.zero;
    [SerializeField] private bool _lookAtTransform;
    [SerializeField, ShowIf("_lookAtTransform")] private Transform _lookAtTarget;
    [SerializeField, ShowIf("_lookAtTransform")] private float _lookAtTargetWeight = 1;
    [SerializeField] private bool _raycastCheck;
    [SerializeField, ShowIf("_raycastCheck")] private Transform _raycastOrigin;
    [SerializeField, ShowIf("_raycastCheck")] private float _minDistFromClosestWall = 0.2f;
    [SerializeField, ShowIf("_raycastCheck")] private LayerMask _raycastLayer = 1;

    [Header("Debug")]
    [SerializeField, ShowIf("_move", "_maintainOriginalOffset"), ReadOnly] private Vector3 _originalOffset;

    private void OnEnable()
    {
        CameraAngleController.BeforeCameraMove += BeforeCameraMove;
    }
    
    private void OnDisable()
    {
        CameraAngleController.BeforeCameraMove -= BeforeCameraMove;
    }
    
    private void Start()
    {
        _originalOffset = transform.position - _objToFollow.position;
    }

    private void BeforeCameraMove()
    {
        Follow();
    }
    
    private void Follow()
    { 
        if (_move)
        {
            var goal = _objToFollow.TransformPoint(_localPositionOffset) + _worldPositionOffset;
            if (_maintainOriginalOffset) goal += _originalOffset;
            transform.position = Vector3.Lerp(transform.position, goal, Time.deltaTime * _moveSpeed);
        }
        if (_rotate)
        {
            var goal = _objToFollow.rotation * Quaternion.Euler(_rotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, goal, Time.deltaTime * _rotateSpeed);
        }
        if (_lookAtTransform)
        {
            var originalRot = transform.rotation;
            transform.LookAt(_lookAtTarget);
            transform.rotation = Quaternion.Slerp(originalRot, transform.rotation, _lookAtTargetWeight);
        }
        if (_raycastCheck)
        {
            var origin = _raycastOrigin.position;
            var dir = (transform.position - origin).normalized;
            var dist = (transform.position - origin).magnitude;
            Physics.Raycast(origin, dir, out var hit,  dist, _raycastLayer);
            if (hit.collider)
            {
                transform.position = origin + dir * (hit.distance - _minDistFromClosestWall);
            }
        }
    }
}
