using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



namespace SoundSystem
{ 
    [CreateAssetMenu(menuName = "SoundSystem/SFX Event", fileName = "SFX_")]
    public class SFXEvent : ScriptableObject
    {
        //music tracks
        [SerializeField] AudioClip _SFXSound;
        //mixer group
        [SerializeField] AudioMixerGroup _mixer;

        [SerializeField] bool isLooping;

        [Range(0f, 1f)]
        public float Volume = 1;

        [Range(-3f, 3f)]
        public float maxPitch = 1f;

        [Range(-3f, 3f)]
        public float minPitch = 0.8f;

        [Range(-1f, 1f)]
        public float panStereo = 0;


        [Range(0f, 1f)]
        [SerializeField] float _spatialBlend;

        [SerializeField] float _startTime;

        //getters
        public AudioClip SFXSound => _SFXSound;
        public float SpatialSound => _spatialBlend;
        public AudioMixerGroup Mixer => _mixer;

        public float StartTime => _startTime;
        public bool IsLooping => isLooping;


        public void Play()
        {
            SFXManager.Instance.PlaySFX(this);
        }
    }
}
