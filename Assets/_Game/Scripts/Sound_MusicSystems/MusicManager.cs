using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace SoundSystem
{
    public class MusicManager : MonoBehaviour
    {

        int _activeLayerIndex = 0;
        int _previosLayerIndex;

        public int ActiveLayerIndex => _activeLayerIndex;
        MusicPlayer _musicPlayer1;
        MusicPlayer _musicPlayer2;

        bool _isMusicPlayer1Playing = false;

        public MusicPlayer ActivePlayer => (_isMusicPlayer1Playing) ? _musicPlayer1 : _musicPlayer2;
        public MusicPlayer InActivePlayer => (_isMusicPlayer1Playing) ? _musicPlayer2 : _musicPlayer1;

        MusicEvent _activeMusicEvent;
        MusicEvent _previousMusicEvent;

        public const int MaxLayerCount = 4;

        float _fadeTime;

        #region hooking up to CGSC

        private void OnEnable()
        {
            CGSC.OnPause += onPause;
            CGSC.OnUnpause += OnUnPause;
            CGSC.OnWinGame += OnWin;
        }

        private void OnDisable()
        {
            CGSC.OnPause -= onPause;
            CGSC.OnUnpause -= OnUnPause;
            CGSC.OnWinGame -= OnWin;
        }

        #endregion

        private static MusicManager _instance;
        public static MusicManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MusicManager>();
                    if (_instance == null)
                    {
                        GameObject singletonGO = new GameObject("MusicManger_singleton");
                        _instance = singletonGO.AddComponent<MusicManager>();

                        DontDestroyOnLoad(singletonGO);
                    }
                }

                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            SetUpMusicPlayer();
        }

        void SetUpMusicPlayer()
        {
            _musicPlayer1 = gameObject.AddComponent<MusicPlayer>();
            _musicPlayer2 = gameObject.AddComponent<MusicPlayer>();
        }

        public void PlayMusic(MusicEvent musicEvent, float fadeTime)

        {
            _activeLayerIndex = 0;
            _fadeTime = fadeTime;
            if (musicEvent == null) return;
            //checking if this is already the music playing
            if (musicEvent == _activeMusicEvent) return;
            //If this is the active music event put needs to stop fade it out
            if(_activeMusicEvent != null)
                ActivePlayer.Stop(fadeTime);
            //setting the active playing music event to this and playing it
            _previousMusicEvent = _activeMusicEvent;
            if (_previousMusicEvent != null && _previousMusicEvent.name == "MUS_Pause")
            {
                _activeLayerIndex = _previosLayerIndex;
            }
            _activeMusicEvent = musicEvent;
            _isMusicPlayer1Playing = !_isMusicPlayer1Playing;

            ActivePlayer.Play(musicEvent, fadeTime);
        }

        public void StopMusic(float fadeTime)
        {
            if (_activeMusicEvent == null) return;

            _activeMusicEvent = null;
            ActivePlayer.Stop(fadeTime);
        }

        public void IncreaseLayerIndex(float fadeTime)
        {
            int newLayerIndex = _activeLayerIndex + 1;
            newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

            if (newLayerIndex == _activeLayerIndex)
                return;

            _activeLayerIndex = newLayerIndex;
            ActivePlayer.FadeVolume(_activeMusicEvent.VolumeToFadeTo, fadeTime);
        }

        public void DecreaseLayerIndex(float fadeTime)
        {
            int newLayerIndex = _activeLayerIndex - 1;
            newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

            if (newLayerIndex == _activeLayerIndex)
                return;

            _activeLayerIndex = newLayerIndex;
            ActivePlayer.FadeVolume(_activeMusicEvent.VolumeToFadeTo, fadeTime);
        }

        private void onPause()
        {
            _previosLayerIndex = _activeLayerIndex;
            _activeLayerIndex = -1;
            IncreaseLayerIndex(1f);
        }
        private void OnUnPause()
        {
            PlayMusic(_previousMusicEvent, 1f);
        }
        private void OnWin()
        {

        }

        public void EndDrums()
        {
            ActivePlayer.drumsEnd();
        }
    }
}
