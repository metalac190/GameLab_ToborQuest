using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborSound : MonoBehaviour
{
    float _speed => gameObject.GetComponent<MovementControls>().Speed;
    [SerializeField] AudioSource _engineSound;

    [Range(-3f, 3f)]
    [SerializeField] float _enginePitchStart;
    [Range(-3f, 3f)]
    [SerializeField] float _MinPitch;
    [Range(-3f, 3f)]
    [SerializeField] float _MaxPitch;




    private void Awake()
    {
        _engineSound.pitch = _enginePitchStart;
    }

    private void Update()
    {
        _engineSound.pitch = Mathf.Lerp(_MinPitch, _MaxPitch, _speed);
    }

}
