using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToFracture : MonoBehaviour
{
    [SerializeField] private GameObject _brokenVersion;
    [SerializeField] private GameObject _unbrokenVersion;
    [SerializeField] private FixedJoint _joint;
    [SerializeField, ReadOnly] private bool _isFractured = false;
    
    private void OnValidate()
    {       
        if (_joint == null) _joint = GetComponent<FixedJoint>();
    }

    private void Start() 
    {
        _unbrokenVersion.SetActive(true);
        _brokenVersion.SetActive(false);   
        _isFractured = false; 
    }

    private void Update()
    {
        if (_joint == null) SetFractured();
    }

    private void SetFractured()
    {
        if (!_isFractured)
        {
            _unbrokenVersion.SetActive(false);
            _brokenVersion.SetActive(true);
            _isFractured = true; 
            Destroy(gameObject);
        }
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
