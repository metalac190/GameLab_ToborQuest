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
    [SerializeField] SFXEvent LandingImpact;
    [SerializeField] SFXEvent metalHit;
    [SerializeField] SFXEvent Roll;
    [SerializeField] SFXEvent EngineCoolDown;

    [SerializeField] SFXEvent pauseSound;
    [SerializeField] SFXEvent unPauseSound;
    [SerializeField] SFXEvent winSound;

    bool _won;

    bool _FlipSoundPlayed;
    bool _boosted;
    float _coolDownTime;
    float _impactSoundCoolDown;

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
        _FlipSoundPlayed = false;
        _boosted = false;

        _driftSound.Stop();
        _boostSound.Stop();
        _won = false;

    }

    private void Update()
    {
        if (_mc.IsGrounded || _mc.IsBoosting)
        {
            _engineSound.pitch = Mathf.Clamp(Mathf.Log(_tobor.velocity.magnitude), _MinPitch, _MaxPitch);
        }
        
        if (_tobor.GetComponent<MovementController>().IsDrifting == true)
        {
            if (_driftSound.isPlaying == false && _won == false)
            {
                _driftSound.time = 0.2f;
                _driftSound.volume = 1;
                _driftSound.Play();
            }
        }


        if (_tobor.GetComponent<MovementController>().IsBoosting == true)
        {
            if (_boostSound.isPlaying == false && _won == false)
            {
                _boostSound.time = 0f;
                _boostSound.volume = 1;
                _boostSound.Play();
            }
        }

        if (_tobor.GetComponent<MovementController>().IsFlipping == false)
        {
            _FlipSoundPlayed = false;
        }
        if (_tobor.GetComponent<MovementController>().IsFlipping == true)
        {
            if (_FlipSoundPlayed == false)
            {
                Roll.Play();
                _FlipSoundPlayed = true;
            }
        }

        if (_tobor.GetComponent<MovementController>().IsBoosting == true)
        {
            _boosted = true;
        }
        if (_tobor.GetComponent<MovementController>().IsBoosting == false)
        {
            if (_boosted == true && _coolDownTime <= Time.time)
            {
                EngineCoolDown.Play();
                _boosted = false;
                _coolDownTime = Time.time + 1f;
            }
        }


    }

    void OnWin()
    {
        PauseAudioSources();
        winSound.Play();
        _won = true;
    }

    void onPause()
    {
        PauseAudioSources();
        pauseSound.Play();
    }

    void OnUnPause()
    {
        _engineSound.Play();
        _driftSound.Play();
        _boostSound.Play();
        unPauseSound.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Time.time >= 1)
        {
            if (other.gameObject.layer == 18)
            {
                metalHit.Volume = Mathf.Clamp(_tobor.GetComponent<Rigidbody>().velocity.magnitude, 0, 1);
                metalHit.Play();
            }
            else if (other.gameObject.layer == 12 || other.gameObject.layer == 0)
            {
                WallHit.Volume = Mathf.Clamp(_tobor.GetComponent<Rigidbody>().velocity.magnitude, 0, 1);
                WallHit.Play();
            }
            else if (other.gameObject.layer == 11)
            {
                if (Time.time >= _impactSoundCoolDown)
                {
                    _impactSoundCoolDown = Time.time + 1;
                    LandingImpact.Volume = Mathf.Clamp(_tobor.GetComponent<Rigidbody>().velocity.magnitude, 0, 1);
                    LandingImpact.Play();
                }
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
