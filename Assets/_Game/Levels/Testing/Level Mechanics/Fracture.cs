using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [SerializeField] private GameObject _brokenVersion;
    [SerializeField] private GameObject _unbrokenVersion;

    void OnValidate()
    {
        if (_unbrokenVersion == null)
        {
            _unbrokenVersion = transform.GetChild(0).gameObject;
        }
        if (_brokenVersion == null)
        {
            _brokenVersion = transform.GetChild(1).gameObject;
        }

    }

    private void Start() 
    {
        _unbrokenVersion.SetActive(true);
        _brokenVersion.SetActive(false);    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MovementController>())
        {
            _unbrokenVersion.SetActive(false);
            _brokenVersion.SetActive(true);
        }
    }


}
