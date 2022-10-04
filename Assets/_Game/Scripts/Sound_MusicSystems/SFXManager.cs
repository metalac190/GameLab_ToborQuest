using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{ 
    public class SFXManager : MonoBehaviour
    {
        int _activeLayerIndex = 0;
        public int ActiveLayerIndex => _activeLayerIndex;

        public SFXPlayer sfxPlayer;
        SFXManager _sfxManager;

        public const int MaxLayerCount = 1;

        float _volume = 1;
        public float Volume
        {
            get => _volume;
            private set
            {
                value = Mathf.Clamp(value, 0, 1);
                _volume = value;
            }
        }

        void SetUpSFXPlayer(GameObject parent)
        {
            sfxPlayer = parent.AddComponent<SFXPlayer>();
        }

        public void PlaySFX(SFXEvent sfxEvent, GameObject parent)
        {

            SetUpSFXPlayer(parent);
            //here
            sfxPlayer.Play(sfxEvent, parent);
        }
    }
}
