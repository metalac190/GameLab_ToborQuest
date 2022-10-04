using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ToborSound : MonoBehaviour
{
    [SerializeField] AudioSource _engineSound;
    [SerializeField] AudioMixerGroup _mixer;

    [Range(0f, 4f)]
    [SerializeField] float _MinPitch;
    [Range(0f, 4f)]
    [SerializeField] float _MaxPitch;
    Rigidbody _tobor;

    #region hooking up to CGSC

    private void OnEnable()
    {
        CGSC.OnPause += onPause;
    }

    private void OnDisable()
    {
        CGSC.OnUnpause += OnUnPause;
    }

    #endregion
    private void Awake()
    {
        _tobor = gameObject.GetComponent<Rigidbody>();
        _engineSound.outputAudioMixerGroup = _mixer;
    }

    private void Update()
    {
        _engineSound.pitch = Mathf.Clamp(Mathf.Log(_tobor.velocity.magnitude), _MinPitch, _MaxPitch);
    }

    void onPause()
    {
        _engineSound.Pause();
    }

    void OnUnPause()
    {
        _engineSound.Play();
    }

}
