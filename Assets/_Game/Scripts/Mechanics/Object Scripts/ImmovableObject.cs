using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ImmovableObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;        
    }
}
