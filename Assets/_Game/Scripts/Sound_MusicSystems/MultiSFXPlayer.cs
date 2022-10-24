using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundSystem
{
    public class MultiSFXPlayer : MonoBehaviour
    {
        MultiSFXEvent _MsfxEvent = null;
        AudioSource _sfxSound;
        AudioSource[] _sfxSounds;
        float timer;
        GameObject _soundOBJ;

        Scene _sceneSpawnedIn;

        private void Start()
        {
            _sceneSpawnedIn = SceneManager.GetActiveScene();
        }

        public void Play(MultiSFXEvent multiSFXEvent, GameObject soundOBJ)
        {
            if (_MsfxEvent.SFXType == SFXType.Random)
            {
                _soundOBJ = soundOBJ;
                _MsfxEvent = multiSFXEvent;
                _sfxSound = soundOBJ.AddComponent<AudioSource>();
                _sfxSound.clip = _MsfxEvent.SFXSounds[Random.Range(0, _MsfxEvent.SFXSounds.Length)];
                _sfxSound.outputAudioMixerGroup = _MsfxEvent.Mixer;
                _sfxSound.loop = _MsfxEvent.IsLooping;
                _sfxSound.volume = _MsfxEvent.Volume;
                _sfxSound.pitch = Random.Range(_MsfxEvent.minPitch, _MsfxEvent.maxPitch);
                _sfxSound.spatialBlend = _MsfxEvent.SpatialSound;
                _sfxSound.panStereo = _MsfxEvent.panStereo;
                _sfxSound.time = _MsfxEvent.StartTime;
                _sfxSound.Play();
            }

            if (_MsfxEvent.SFXType == SFXType.together)
            {
                for (int i = 0; i <= _MsfxEvent.SFXSounds.Length; i++)
                {
                    _soundOBJ = soundOBJ;
                    _MsfxEvent = multiSFXEvent;
                    _sfxSounds[i] = soundOBJ.AddComponent<AudioSource>();
                    _sfxSounds[i].clip = _MsfxEvent.SFXSounds[i];
                    _sfxSounds[i].outputAudioMixerGroup = _MsfxEvent.Mixer;
                    _sfxSounds[i].loop = _MsfxEvent.IsLooping;
                    _sfxSounds[i].volume = _MsfxEvent.Volume;
                    _sfxSounds[i].pitch = Random.Range(_MsfxEvent.minPitch, _MsfxEvent.maxPitch);
                    _sfxSounds[i].spatialBlend = _MsfxEvent.SpatialSound;
                    _sfxSounds[i].panStereo = _MsfxEvent.panStereo;
                    _sfxSounds[i].time = _MsfxEvent.StartTime;
                    _sfxSounds[i].Play();
                }
            }
        }

        public void Stop()
        {
            Destroy(_soundOBJ);
        }

        public void Update()
        {
            if (_MsfxEvent.SFXType == SFXType.Random)
            {
                if (_sfxSound.isPlaying != true)
                {
                    Stop();
                }
            }
            if (_MsfxEvent.SFXType == SFXType.together)
            {
                for (int i = 0; i <= _MsfxEvent.SFXSounds.Length; i++)
                {
                    if(_sfxSounds[i].isPlaying != true)
                    {
                        Destroy(_sfxSounds[i]);
                    }
                }
                if (_sfxSounds == null)
                {
                    Stop();
                }
            }
            if (_sceneSpawnedIn != SceneManager.GetActiveScene() && _MsfxEvent.IsLooping == true)
            {
                Stop();
            }
        }
    }
}
