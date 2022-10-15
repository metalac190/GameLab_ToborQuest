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
        public float Volume;

        [Range(-3f, 3f)]
        public float Pitch;


        [Range(0f, 1f)]
        [SerializeField] float _spatialSound;

        [SerializeField] float _playTime;

        //getters
        public AudioClip SFXSound => _SFXSound;
        public float SpatialSound => _spatialSound;
        public AudioMixerGroup Mixer => _mixer;

        public float PlayTime => _playTime;
        public bool IsLooping => isLooping;

        public void Play()
        {
            GameObject soundOBJ = new GameObject("SFX" + this.name);
            soundOBJ.AddComponent<SFXManager>().PlaySFX(this, soundOBJ);
        }
    }
}
