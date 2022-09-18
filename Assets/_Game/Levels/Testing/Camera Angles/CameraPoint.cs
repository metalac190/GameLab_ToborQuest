using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath _follow;

    public CinemachineSmoothPath Follow => _follow;
    
    private CameraPointCloud _cloud;

    public void SetCloud(CameraPointCloud cloud) => _cloud = cloud;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Tobor"))
        {
            _cloud.SetActiveCamera(this);
        }
    }
}
