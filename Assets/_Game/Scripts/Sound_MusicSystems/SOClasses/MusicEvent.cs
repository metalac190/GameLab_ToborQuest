using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public enum LayerType
    {
        //fade in each layer of music and stack them. Like if their are multiple instruments that come in and out
        Additive,
        //activate one layer fade it in/out fadein another and so on
        Single,

    }

    [CreateAssetMenu(menuName = "SoundSystem/Music Event", fileName = "MUS_")]
    public class MusicEvent : ScriptableObject
    {
        //music tracks
        [SerializeField] AudioClip[] _musicLayers;
        //blend type
        [SerializeField] LayerType _layerType = LayerType.Additive;
        //mixer group
        [SerializeField] AudioMixerGroup _mixer;

        [SerializeField] float fadeTime;

        [Range(0f, 1f)]
        public float VolumeToFadeTo = 1;

        //getters
        public AudioClip[] MusicLayers => _musicLayers;
        public LayerType LayerType => _layerType;
        public AudioMixerGroup Mixer => _mixer;

        public void Play()
        {
            MusicManager.Instance.PlayMusic(this, fadeTime);
        }
    }
}
