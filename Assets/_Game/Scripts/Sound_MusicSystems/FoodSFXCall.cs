using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class FoodSFXCall : MonoBehaviour
{
    [SerializeField] SFXEvent SFXEventCall;
    bool _played = false;
    float _timer;

    private void Awake()
    {
        _timer = Time.time + .1f;
    }
    private void OnEnable()
    {
        _played = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (_timer <= Time.time)
        {
            if (other.gameObject.layer != 10 && _played == false)
            {
                int randCheck = Random.Range(0, 2);
                if (_played == false && randCheck == 1)
                {
                    SFXEventCall.Volume = 0.8f;
                    SFXEventCall.Play();
                }
                _played = true;
            }
        }
    }
}
