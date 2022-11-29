using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class AmbientCaller : MonoBehaviour
{
    float _timer;
    [SerializeField] AmbientEvent _ambienceToPlay;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;

    private void Start()
    {
        randomTimerSet();
    }
    // Update is called once per frame
    void Update()
    {
        if (_timer <= Time.time)
        {
            _ambienceToPlay.Play(this.gameObject);
            randomTimerSet();
        }
    }

    void randomTimerSet()
    {
        _timer = Time.time + Random.Range(minWaitTime, maxWaitTime);
    }
}
