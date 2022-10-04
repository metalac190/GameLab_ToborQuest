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

        public void Play(SFXEvent sfxEvent, GameObject parent)
        {
            _sfxEvent = sfxEvent;
            _sfxSound = parent.AddComponent<AudioSource>();
            _sfxSound.clip = sfxEvent.SFXSound;
            _sfxSound.outputAudioMixerGroup = sfxEvent.Mixer;
            _sfxSound.loop = sfxEvent.IsLooping;
            _sfxSound.volume = sfxEvent.Volume;
            _sfxSound.pitch = sfxEvent.Pitch;
            _sfxSound.Play();

            if (_sfxEvent.PlayTime <= 0)
            {
                print("PLAYTIME IS SET TO ZERO");
            }

            StartCoroutine(waitRoutine(parent));
        }

        public void Stop(GameObject parent)
        {
            Destroy(_sfxSound);
            Destroy(parent.GetComponent<SFXPlayer>());
            Destroy(parent.GetComponent<SFXManager>());
        }

        IEnumerator waitRoutine(GameObject parent)
        {
            yield return new WaitForSeconds(_sfxEvent.PlayTime);
            Stop(parent);
        }
    }
}
