using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{ 
    public class SFXPlayer : MonoBehaviour
    {
        SFXEvent _sfxEvent = null;
        AudioSource _sfxSound;
        float timer;

        public void Play(SFXEvent sfxEvent, GameObject soundOBJ)
        {
            _sfxEvent = sfxEvent;
            _sfxSound = soundOBJ.AddComponent<AudioSource>();
            _sfxSound.clip = sfxEvent.SFXSound;
            _sfxSound.outputAudioMixerGroup = sfxEvent.Mixer;
            _sfxSound.loop = sfxEvent.IsLooping;
            _sfxSound.volume = sfxEvent.Volume;
            _sfxSound.pitch = sfxEvent.Pitch;
            _sfxSound.spatialBlend = sfxEvent.SpatialSound;
            _sfxSound.panStereo = sfxEvent.panStereo;
            _sfxSound.time = _sfxEvent.StartTime;
            _sfxSound.Play();
        }

        public void Stop()
        {
            Destroy(gameObject);
        }

        public void Update()
        {
            if (_sfxSound.isPlaying != true)
            {
                Stop();
            }
        }
    }
}
