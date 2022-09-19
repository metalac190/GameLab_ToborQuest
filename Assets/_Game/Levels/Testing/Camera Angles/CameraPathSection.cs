using System;
using Cinemachine;
using UnityEngine;

public class CameraPathSection : MonoBehaviour
{
    [SerializeField] private CameraPathController _controller;
    [SerializeField] private CinemachineSmoothPath _path;
    [SerializeField] private bool _after;
    
    [Header("Before")]
    [SerializeField] private float _dollyOffsetBefore = -0.5f;
    [SerializeField] private TriggerDetectPlayer _triggerBefore;
    
    [Header("After")]
    [SerializeField] private float _dollyOffsetAfter = 0.5f;
    [SerializeField] private TriggerDetectPlayer _triggerAfter;
    
    private void OnEnable()
    {
        if (_triggerBefore) _triggerBefore.OnPlayerEnter += OnTriggerBefore;
        if (_triggerAfter) _triggerAfter.OnPlayerEnter += OnTriggerAfter;
    }

    private void OnDisable()
    {
        if (_triggerBefore) _triggerBefore.OnPlayerEnter -= OnTriggerBefore;
        if (_triggerAfter) _triggerAfter.OnPlayerEnter -= OnTriggerAfter;
    }

    private void Awake()
    {
        UpdatePoint(_after);
    }

    private void OnValidate()
    {
        _controller = transform.parent.GetComponent<CameraPathController>();
        foreach (Transform child in transform)
        {
            var childName = child.name.ToLower();
            if (_path == null && childName.Contains("dolly"))
            {
                _path = child.GetComponent<CinemachineSmoothPath>();
            }
            if (_triggerBefore == null && childName.Contains("before"))
            {
                _triggerBefore = child.GetComponent<TriggerDetectPlayer>();
            }
            if (_triggerAfter == null && childName.Contains("after"))
            {
                _triggerAfter = child.GetComponent<TriggerDetectPlayer>();
            }
        }
    }

    public void UpdatePoint(bool after)
    {
        _after = after;
        if (_triggerBefore) _triggerBefore.gameObject.SetActive(!after);
        if (_triggerAfter) _triggerAfter.gameObject.SetActive(after);
    }

    private void OnTriggerBefore()
    {
        _after = false;
        ActivateCameraPath();
    }

    private void OnTriggerAfter()
    {
        _after = true;
        ActivateCameraPath();
    }

    private void ActivateCameraPath()
    {
        var offset = _after ? _dollyOffsetAfter : _dollyOffsetBefore;
        _controller.SetActiveCamera(this, _path, offset);
        if (_triggerBefore) _triggerBefore.gameObject.SetActive(false);
        if (_triggerAfter) _triggerAfter.gameObject.SetActive(false);
    }
}
