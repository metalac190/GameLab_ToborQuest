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

        Scene _sceneSpawnedIn;

        private void Awake()
        {
            _sfxSound = this.gameObject.GetComponent<AudioSource>();
        }

        public void Play(SFXEvent sfxEvent)
        {
            
            _sfxEvent = sfxEvent;
            _sfxSound.clip = sfxEvent.SFXSound;
            _sfxSound.outputAudioMixerGroup = sfxEvent.Mixer;
            _sfxSound.loop = sfxEvent.IsLooping;
            _sfxSound.volume = sfxEvent.Volume;
            _sfxSound.pitch = Random.Range(sfxEvent.minPitch, sfxEvent.maxPitch);
            _sfxSound.spatialBlend = sfxEvent.SpatialSound;
            _sfxSound.panStereo = sfxEvent.panStereo;
            _sfxSound.time = _sfxEvent.StartTime;
            _sfxSound.Play();
            _sceneSpawnedIn = SceneManager.GetActiveScene();
        }

        public void Stop()
        {
            _sfxSound.clip = null;
            _sceneSpawnedIn = SceneManager.GetActiveScene();
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
