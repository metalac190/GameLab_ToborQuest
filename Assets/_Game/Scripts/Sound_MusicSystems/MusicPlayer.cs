using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoundSystem
{
    public class MusicPlayer : MonoBehaviour
    {
        List<AudioSource> _layerSources = new List<AudioSource>();
        List<float> _sourceStartVolumes = new List<float>();
        MusicEvent _musicEvent = null;

        Coroutine _fadeVolumeRoutine = null;
        Coroutine _stopRountine = null;

        private void Awake()
        {
            CreateLayerSources();
        }

        void CreateLayerSources()
        {
            for (int i = 0; i < MusicManager.MaxLayerCount; i++)
            {
                _layerSources.Add(gameObject.AddComponent<AudioSource>());

                _layerSources[i].playOnAwake = false;
                _layerSources[i].loop = true;
            }
        }

        public void Play(MusicEvent musicEvent, float fadeTime)
        {
            _musicEvent = musicEvent;
            for (int i = 0; i < _layerSources.Count 
                && (i < musicEvent.MusicLayers.Length); i++)
            {
                if (musicEvent.MusicLayers[i] != null)
                {
                    _layerSources[i].volume = 0;
                    _layerSources[i].clip = musicEvent.MusicLayers[i];
                    _layerSources[i].outputAudioMixerGroup = musicEvent.Mixer;
                    _layerSources[i].Play();
                }
            }
            FadeVolume(musicEvent.VolumeToFadeTo, fadeTime);
        }

        public void Stop(float fadeTime)
        {
            if (_stopRountine != null)
                StopCoroutine(_stopRountine);
            _stopRountine = StartCoroutine(StopRoutine(fadeTime));
        }

        IEnumerator StopRoutine(float fadeTime)
        {
            if (_fadeVolumeRoutine != null)
                StopCoroutine(_fadeVolumeRoutine);
            if (_musicEvent.LayerType == LayerType.Additive)
            {
                _fadeVolumeRoutine = StartCoroutine(LerpSourceAdditiveRoutine(0, fadeTime));
            }
            else if (_musicEvent.LayerType == LayerType.Single)
            {
                _fadeVolumeRoutine = StartCoroutine(LerpSourcSingleRoutine(0, fadeTime));
            }

            yield return _fadeVolumeRoutine;

            foreach (AudioSource source in _layerSources)
            {
                source.Stop();
            }
        }

        public void FadeVolume(float targetVolume, float fadeTime)
        {
            targetVolume = Mathf.Clamp(targetVolume, 0, 1);
            if (fadeTime < 0) fadeTime = 0;

            if (_fadeVolumeRoutine != null)
                StopCoroutine(_fadeVolumeRoutine);

            if (_musicEvent.LayerType == LayerType.Additive)
            {
               _fadeVolumeRoutine =  StartCoroutine
                    (LerpSourceAdditiveRoutine(targetVolume, fadeTime));
            }
            else if (_musicEvent.LayerType == LayerType.Single)
            {
                _fadeVolumeRoutine = StartCoroutine
                    (LerpSourcSingleRoutine( targetVolume, fadeTime));
            }
        }
        IEnumerator LerpSourceAdditiveRoutine(float targetVolume, float fadeTime)
        {
            SaveSourceStartVolumes();

            float newVolume;
            float startVolume;

            for (float elapsedTime = 0; elapsedTime <= fadeTime; elapsedTime += Time.deltaTime)
            {
                for (int i = 0; i < _layerSources.Count; i++)
                {
                    if (i <= MusicManager.Instance.ActiveLayerIndex)
                    {
                        startVolume = _sourceStartVolumes[i];
                        newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                        _layerSources[i].volume = newVolume;
                    }
                    else 
                    {
                        startVolume = _sourceStartVolumes[i];
                        newVolume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeTime);
                        _layerSources[i].volume = newVolume;
                    }
                }

                yield return null;
            }
            for (int i = 0; i < _layerSources.Count; i++)
            {
                if (i <= MusicManager.Instance.ActiveLayerIndex)
                {
                    _layerSources[i].volume = targetVolume;
                }
                else
                {
                    _layerSources[i].volume = 0;
                    _layerSources[i].time = 0;
                }
            }
        }
        IEnumerator LerpSourcSingleRoutine(float targetVolume, float fadeTime)
        {
            SaveSourceStartVolumes();

            float newVolume;
            float startVolume;
            for (float elapsedTime = 0; elapsedTime <= fadeTime; elapsedTime += 0.005f)
            {
                for (int i = 0; i < _layerSources.Count; i++)
                {
                    if (i == MusicManager.Instance.ActiveLayerIndex)
                    {
                        startVolume = _sourceStartVolumes[i];
                        newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                        _layerSources[i].volume = newVolume;
                    }
                    else
                    {
                        startVolume = _sourceStartVolumes[i];
                        newVolume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeTime);
                        _layerSources[i].volume = newVolume;
                    }
                }

                yield return null;
            }
            for (int i = 0; i < _layerSources.Count; i++)
            {
                if (i == MusicManager.Instance.ActiveLayerIndex)
                {
                    _layerSources[i].volume = targetVolume;
                }
                else
                {
                    _layerSources[i].volume = 0;
                    _layerSources[i].time = 0;
                }
            }
        }


        private void SaveSourceStartVolumes()
        {
            _sourceStartVolumes.Clear();
            for (int i = 0; i < _layerSources.Count; i++)
            {
                _sourceStartVolumes.Add(_layerSources[i].volume);
            }
        }
    }
}
