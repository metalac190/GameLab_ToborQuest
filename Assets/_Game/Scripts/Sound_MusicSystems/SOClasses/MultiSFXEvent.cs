using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



namespace SoundSystem
{
    public enum SFXType
    {
        //choose to play one random sound from the list
        Random,
        //play all given sounds at once
        together,

    }

    [CreateAssetMenu(menuName = "SoundSystem/Multi SFX Event", fileName = "MSFX_")]
    public class MultiSFXEvent : ScriptableObject
    {
        //music tracks
        [SerializeField] AudioClip[] _SFXSounds;
        //blend type
        [SerializeField] SFXType _sfxType = SFXType.Random;
        //mixer group
        [SerializeField] AudioMixerGroup _mixer;

        [SerializeField] bool isLooping;

        [Range(0f, 1f)]
        public float Volume = 1;

        [Range(-3f, 3f)]
        public float maxPitch = 1f;

        [Range(-3f, 3f)]
        public float minPitch = 0.9f;

        [Range(-1f, 1f)]
        public float panStereo = 0;


        [Range(0f, 1f)]
        [SerializeField] float _spatialBlend;

        [SerializeField] float _startTime;

        //getters
        public AudioClip[] SFXSounds => _SFXSounds;
        public float SpatialSound => _spatialBlend;
        public AudioMixerGroup Mixer => _mixer;

        public float StartTime => _startTime;
        public bool IsLooping => isLooping;

        public SFXType SFXType => _sfxType;


        public void Play()
        {
            GameObject soundOBJ = new GameObject("SFX" + this.name);
            SFXManager.Instance.PlayMultiSFX(this, soundOBJ);
        }
    }
}
