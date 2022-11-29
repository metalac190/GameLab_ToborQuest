using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    [CreateAssetMenu(menuName = "SoundSystem/Ambient Event", fileName = "AMB_")]
    public class AmbientEvent : ScriptableObject
    {
        //music tracks
        [SerializeField] AudioClip _AMBSound;
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

        [SerializeField] float _startTime;

        [Range (0f, 5f)]
        [SerializeField] float _doplerLevel = 1;

        [SerializeField] float _minDistance = 1;
        [SerializeField] float _maxDistance = 500;

        //getters
        public AudioClip AMBSound => _AMBSound;
        public AudioMixerGroup Mixer => _mixer;

        public float StartTime => _startTime;
        public bool IsLooping => isLooping;
        public float DoplerLevel => _doplerLevel;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;

        public void Play(GameObject parent)
        {
            AmbientManager.Instance.PlayAMB(this, parent);
        }
    }
}
