using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToFracture : MonoBehaviour
{
    [SerializeField] private GameObject _brokenVersion;
    [SerializeField] private GameObject _unbrokenVersion;
    [SerializeField] private FixedJoint _joint;
    
    private void OnValidate()
    {
        if (_unbrokenVersion == null)
        {
            _unbrokenVersion = transform.GetChild(0).gameObject;
        }
        if (_brokenVersion == null)
        {
            _brokenVersion = transform.GetChild(1).gameObject;
        }
        if (_joint == null) _joint = GetComponent<FixedJoint>();
    }

    private void Start() 
    {
        _unbrokenVersion.SetActive(true);
        _brokenVersion.SetActive(false);    
    }

    private void Update()
    {
        if (_joint == null) SetFractured();
    }

    private void SetFractured()
    {
        _unbrokenVersion.SetActive(false);
        _brokenVersion.SetActive(true);
        Destroy(gameObject);
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MovementController>())
        {
            SetFractured();
        }
    }
    */
}
