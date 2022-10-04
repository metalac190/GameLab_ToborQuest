using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



namespace SoundSystem
{ 
    public enum SFXLayerType
    {
        //2D mainly for ui and other such things
        twoD,
        //3D sounds like in world
        threeD,

    }

    [CreateAssetMenu(menuName = "SoundSystem/SFX Event", fileName = "SFX_")]
    public class SFXEvent : ScriptableObject
    {
        //music tracks
        [SerializeField] AudioClip _SFXSound;
        //blend type
        [SerializeField] SFXLayerType _layerType = SFXLayerType.twoD;
        //mixer group
        [SerializeField] AudioMixerGroup _mixer;

        [SerializeField] bool isLooping;

        [Range(0f, 1f)]
        public float Volume;

        [Range(-3f, 3f)]
        public float Pitch;

        [SerializeField] float _playTime;

        //getters
        public AudioClip SFXSound => _SFXSound;
        public SFXLayerType LayerType => _layerType;
        public AudioMixerGroup Mixer => _mixer;

        public float PlayTime => _playTime;
        public bool IsLooping => isLooping;

        public void Play(GameObject parent)
        {
            parent.AddComponent<SFXManager>().PlaySFX(this, parent);
        }
    }
}
