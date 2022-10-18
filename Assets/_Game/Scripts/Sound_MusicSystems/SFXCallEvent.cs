using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class SFXCallEvent : MonoBehaviour
{
    [SerializeField] SFXEvent SFXEventCall;
    private void OnCollisionEnter(Collision other)
    {
        if (Time.time >= 3)
        {
            if (other.gameObject.layer == 9 || other.gameObject.layer == 10 || other.gameObject.layer == 13)
            {
                SFXEventCall.Play();
            }
        }
    }
}
