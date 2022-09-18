using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPointCloud : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private Transform _cameraPos;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField, ReadOnly] private CameraPoint _activeCamera;
    [SerializeField] private List<CameraPoint> _cameras;

    private void Start()
    {
        foreach (var cam in _cameras)
        {
            cam.SetCloud(this);
        }
        _activeCamera = _cameras[0];
    }

    private void Update()
    {
        var dolly = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_Path = _activeCamera.Follow;
        _cameraPos.position = Vector3.Lerp(_cameraPos.position, _activeCamera.transform.position, _speed * Time.deltaTime);
    }

    public void SetActiveCamera(CameraPoint cam)
    {
        _activeCamera = cam;
    }
}
