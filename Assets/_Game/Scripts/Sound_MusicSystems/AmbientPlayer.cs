using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundSystem
{
    public class AmbientPlayer : MonoBehaviour
    {
        AmbientEvent _ambEvent = null;
        AudioSource _ambSound;

        Scene _sceneSpawnedIn;

        private void Awake()
        {
            _ambSound = this.gameObject.GetComponent<AudioSource>();
        }

        public void Play(AmbientEvent ambEvent)
        {

            _ambEvent = ambEvent;
            _ambSound.clip = ambEvent.AMBSound;
            _ambSound.outputAudioMixerGroup = ambEvent.Mixer;
            _ambSound.loop = ambEvent.IsLooping;
            _ambSound.volume = ambEvent.Volume;
            _ambSound.pitch = Random.Range(ambEvent.minPitch, ambEvent.maxPitch);
            _ambSound.spatialBlend = 1;
            _ambSound.panStereo = ambEvent.panStereo;
            _ambSound.dopplerLevel = ambEvent.DoplerLevel;
            _ambSound.minDistance = ambEvent.MinDistance;
            _ambSound.maxDistance = ambEvent.MaxDistance;
            _ambSound.time = _ambEvent.StartTime;
            _ambSound.Play();
            _sceneSpawnedIn = SceneManager.GetActiveScene();
        }

        public void Stop()
        {
            _ambSound.clip = null;
            _sceneSpawnedIn = SceneManager.GetActiveScene();
        }

        public void Update()
        {
            if (_ambSound.isPlaying != true)
            {
                Stop();
            }
            if (_sceneSpawnedIn != SceneManager.GetActiveScene() && _ambEvent.IsLooping == true)
            {
                Stop();
            }

        }
    }
}
