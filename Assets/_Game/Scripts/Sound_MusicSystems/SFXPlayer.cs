using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundSystem
{ 
    public class SFXPlayer : MonoBehaviour
    {
        SFXEvent _sfxEvent = null;
        AudioSource _sfxSound;
        float timer;
        GameObject _soundOBJ;

        Scene _sceneSpawnedIn;

        private void Start()
        {
            _sceneSpawnedIn = SceneManager.GetActiveScene();
            print(_sceneSpawnedIn);
        }

        public void Play(SFXEvent sfxEvent, GameObject soundOBJ)
        {
            _soundOBJ = soundOBJ;
            _sfxEvent = sfxEvent;
            _sfxSound = soundOBJ.AddComponent<AudioSource>();
            _sfxSound.clip = sfxEvent.SFXSound;
            _sfxSound.outputAudioMixerGroup = sfxEvent.Mixer;
            _sfxSound.loop = sfxEvent.IsLooping;
            _sfxSound.volume = sfxEvent.Volume;
            _sfxSound.pitch = Random.Range(sfxEvent.minPitch, sfxEvent.maxPitch);
            _sfxSound.spatialBlend = sfxEvent.SpatialSound;
            _sfxSound.panStereo = sfxEvent.panStereo;
            _sfxSound.time = _sfxEvent.StartTime;
            _sfxSound.Play();
        }

        public void Stop()
        {
            Destroy(_soundOBJ);
        }

        public void Update()
        {
            if (_sfxSound.isPlaying != true)
            {
                Stop();
            }
            if (_sceneSpawnedIn != SceneManager.GetActiveScene() && _sfxEvent.IsLooping == true)
            {
                Stop();
            }
        }
    }
}
