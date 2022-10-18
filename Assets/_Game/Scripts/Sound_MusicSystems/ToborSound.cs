using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using SoundSystem;

public class ToborSound : MonoBehaviour
{
    [SerializeField] AudioSource _engineSound;
    [SerializeField] AudioMixerGroup _mixer;

    [Range(0f, 4f)]
    [SerializeField] float _MinPitch;
    [Range(0f, 4f)]
    [SerializeField] float _MaxPitch;
    Rigidbody _tobor;

    private MovementController _mc;

    [SerializeField] SFXEvent WallHit;
    [SerializeField] SFXEvent metalHit;

    [SerializeField] SFXEvent _drift;
    [SerializeField] SFXEvent _boost;

    GameObject _driftSound;
    GameObject _boostSound;

    #region hooking up to CGSC

    private void OnEnable()
    {
        CGSC.OnPause += onPause;
        CGSC.OnUnpause += OnUnPause;
    }

    private void OnDisable()
    {
        CGSC.OnPause -= onPause;
        CGSC.OnUnpause -= OnUnPause;
    }

    #endregion
    private void Awake()
    {
        _tobor = gameObject.GetComponent<Rigidbody>();
        _engineSound.outputAudioMixerGroup = _mixer;

        _mc = GetComponent<MovementController>();

    }

    private void Update()
    {
        if (_mc.IsGrounded || _mc.IsBoosting)
        {
            _engineSound.pitch = Mathf.Clamp(Mathf.Log(_tobor.velocity.magnitude), _MinPitch, _MaxPitch);
        }
        if (_tobor.GetComponent<MovementController>().IsDrifting == true)
        {
            if(_driftSound == null)
            {
                _drift.Play();
                _driftSound = GameObject.Find("SFXSFX_Tobor_MoveSlide");
            }
        }
        if (_driftSound != null)
        {
            if (_tobor.GetComponent<MovementController>().IsDrifting == false)
            {
                Destroy(_driftSound);
                _driftSound = null;
            }
        }

        if (_tobor.GetComponent<MovementController>().IsBoosting == true)
        {
            if (_boostSound == null)
            {
                _boost.Play();
                _boostSound = GameObject.Find("SFXSFX_Tobor_RocketThrust");
            }
        }
        if (_boostSound != null)
        {
            if (_tobor.GetComponent<MovementController>().IsBoosting == false)
            {
                Destroy(_boostSound);
                _boostSound = null;
            }
        }
    }

    void onPause()
    {
        _engineSound.Pause();
    }

    void OnUnPause()
    {
        _engineSound.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Time.time >= 1)
        {
            if (other.gameObject.layer == 18)
            {
                metalHit.Play();
            }
            else if (other.gameObject.layer == 12 || other.gameObject.layer == 11 || other.gameObject.layer == 0)
            {
                WallHit.Play();
            }

        }
    }

}
