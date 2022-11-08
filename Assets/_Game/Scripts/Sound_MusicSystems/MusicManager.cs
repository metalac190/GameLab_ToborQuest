using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace SoundSystem
{
    public class MusicManager : MonoBehaviour
    {
        bool _paused;

        int _activeLayerIndex = 0;
        int _priorToPauseLayerIndex;

        public int ActiveLayerIndex => _activeLayerIndex;
        MusicPlayer _musicPlayer1;
        MusicPlayer _musicPlayer2;
        AudioSource[] _musSources;

        bool _isMusicPlayer1Playing = false;

        public MusicPlayer ActivePlayer => (_isMusicPlayer1Playing) ? _musicPlayer1 : _musicPlayer2;
        public MusicPlayer InActivePlayer => (_isMusicPlayer1Playing) ? _musicPlayer2 : _musicPlayer1;

        MusicEvent _activeMusicEvent;

        Scene _songScene;

        public const int MaxLayerCount = 9;

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
        private void Update()
        {
            if (SceneManager.GetActiveScene() != _songScene && _paused == false)
            {
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    _activeLayerIndex = -1;
                    IncreaseLayerIndex(_fadeTime);
                }
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    _activeLayerIndex = 3;
                    IncreaseLayerIndex(_fadeTime);
                }
                if (SceneManager.GetActiveScene().name == "Level 1")
                {
                    _activeLayerIndex = 4;
                    IncreaseLayerIndex(_fadeTime);
                }
                if (SceneManager.GetActiveScene().name == "Level 2")
                {
                    _activeLayerIndex = 3;
                    IncreaseLayerIndex(_fadeTime);
                }
                if (SceneManager.GetActiveScene().name == "Level 3")
                {
                    _activeLayerIndex = 3;
                    IncreaseLayerIndex(_fadeTime);
                }
                if (SceneManager.GetActiveScene().name == "Level 4")
                {
                    _activeLayerIndex = 4;
                    IncreaseLayerIndex(_fadeTime);
                }
                _songScene = SceneManager.GetActiveScene();
            }
            
            songTransitionChecks();
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
            _paused = false;
        }

        void SetUpMusicPlayer()
        {
            _songScene = SceneManager.GetActiveScene();
            _musicPlayer1 = gameObject.AddComponent<MusicPlayer>();
            _musicPlayer2 = gameObject.AddComponent<MusicPlayer>();

            _musSources = GetComponents<AudioSource>();

        }

        public void PlayMusic(MusicEvent musicEvent, float fadeTime)

        {
            _fadeTime = fadeTime;
            if (musicEvent == null) return;
            //checking if this is already the music playing
            if (musicEvent == _activeMusicEvent) return;
            //If this is the active music event put needs to stop fade it out
            if(_activeMusicEvent != null)
                ActivePlayer.Stop(fadeTime);
            //setting the active playing music event to this and playing it
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
            print("yea");
            _paused = true;
            _priorToPauseLayerIndex = _activeLayerIndex;
            _activeLayerIndex = 5;
           IncreaseLayerIndex(2);
        }
        private void OnUnPause()
        {
            _activeLayerIndex = _priorToPauseLayerIndex-1;
           IncreaseLayerIndex(2);
            _paused = false;
        }
        private void OnWin()
        {
            //play win sound
        }

        void songTransitionChecks()
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (_musSources[0].time == 8f)
                {
                    _musSources[0].time = 0;
                    _activeLayerIndex = 1;
                    IncreaseLayerIndex(.5f);
                }
            }
            if (SceneManager.GetActiveScene().name == "Level 1")
            {

            }
            if (SceneManager.GetActiveScene().name == "Level 2")
            {

            }
            if (SceneManager.GetActiveScene().name == "Level 3")
            {

            }
            if (SceneManager.GetActiveScene().name == "Level 4")
            {

            }
        }
    }
}
