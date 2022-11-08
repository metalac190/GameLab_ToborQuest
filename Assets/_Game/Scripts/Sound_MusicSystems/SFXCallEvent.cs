using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class SFXCallEvent : MonoBehaviour
{
    float _delayTime;

    private void Awake()
    {
        _delayTime = Time.time + 1;
    }
    [SerializeField] SFXEvent SFXEventCall;
    private void OnCollisionEnter(Collision other)
    {
        if (Time.time >= _delayTime)
        {
            if (other.gameObject.layer == 9 || other.gameObject.layer == 10 || other.gameObject.layer == 13)
            {
                SFXEventCall.Play();
            }
        }
    }
}
