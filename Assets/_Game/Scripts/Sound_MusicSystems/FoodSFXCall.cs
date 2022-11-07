using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class FoodSFXCall : MonoBehaviour
{
    [SerializeField] SFXEvent SFXEventCall;
    bool _played = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12)
        {
            int randCheck = Random.Range(0, 3);
            if (_played == false && randCheck == 1)
            {
                SFXEventCall.Volume = 0.8f;
                SFXEventCall.Play();
            }
            _played = true;
        }
    }
}
