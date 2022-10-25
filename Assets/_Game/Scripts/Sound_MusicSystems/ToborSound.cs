using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using SoundSystem;

public class ToborSound : MonoBehaviour
{
    [SerializeField] AudioSource _engineSound;
    [SerializeField] AudioSource _driftSound;
    [SerializeField] AudioSource _boostSound;
    [SerializeField] AudioMixerGroup _mixer;

    [Range(0f, 4f)]
    [SerializeField] float _MinPitch;
    [Range(0f, 4f)]
    [SerializeField] float _MaxPitch;
    Rigidbody _tobor;

    private MovementController _mc;

    [SerializeField] SFXEvent WallHit;
    [SerializeField] SFXEvent metalHit;

    bool _won;

    #region hooking up to CGSC

    private void OnEnable()
    {
        CGSC.OnPause += onPause;
        CGSC.OnUnpause += OnUnPause;
        CGSC.OnWinGame += OnWin;
    }

    private void OnDisable()
    {
        CGSC.OnPause -= onPause;
        CGSC.OnUnpause -= OnUnPause;
        CGSC.OnWinGame -= OnWin;
    }

    #endregion
    private void Awake()
    {
        _tobor = gameObject.GetComponent<Rigidbody>();
        _engineSound.outputAudioMixerGroup = _mixer;

        _mc = GetComponent<MovementController>();
        _won = false;

    }

    private void Update()
    {
        if (_mc.IsGrounded || _mc.IsBoosting)
        {
            //_engineSound.pitch = Mathf.Clamp(Mathf.Log(_tobor.velocity.magnitude), _MinPitch, _MaxPitch);
        }
        /*
        if (_tobor.GetComponent<MovementController>().IsDrifting == true)
        {
            _driftSound.Play();
        }


        if (_tobor.GetComponent<MovementController>().IsBoosting == true)
        {
           _boost.Play();
           _boostSound = GameObject.Find("SFXSFX_Tobor_RocketThrust");
        }
        */
        
    }

    void OnWin()
    {
        PauseAudioSources();
    }

    void onPause()
    {
        PauseAudioSources();
    }

    void OnUnPause()
    {
        _engineSound.Play();
        _driftSound.Play();
        _boostSound.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Time.time >= 1)
        {
            if (other.gameObject.layer == 18)
            {
                metalHit.Volume = Mathf.Clamp(_tobor.GetComponent<Rigidbody>().velocity.magnitude ,0, 1);
                metalHit.Play();
            }
            else if (other.gameObject.layer == 12 || other.gameObject.layer == 11 || other.gameObject.layer == 0)
            {
                WallHit.Volume = Mathf.Clamp(_tobor.GetComponent<Rigidbody>().velocity.magnitude, 0, 1);
                WallHit.Play();
            }

        }
    }

    private void PauseAudioSources()
    {
        _engineSound.Pause();
        _driftSound.Pause();
        _boostSound.Pause();
    }

}
