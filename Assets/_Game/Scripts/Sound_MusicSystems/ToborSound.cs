using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborSound : MonoBehaviour
{
    [SerializeField] AudioSource _engineSound;

    [Range(0f, 4f)]
    [SerializeField] float _MinPitch;
    [Range(0f, 4f)]
    [SerializeField] float _MaxPitch;
    Rigidbody _tobor;


    private void Awake()
    {
        _tobor = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _engineSound.pitch = Mathf.Clamp(Mathf.Log(_tobor.velocity.magnitude), _MinPitch, _MaxPitch);
        print(_engineSound.pitch);
    }

}
