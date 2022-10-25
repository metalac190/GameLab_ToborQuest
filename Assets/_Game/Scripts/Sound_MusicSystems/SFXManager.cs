using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{ 
    public class SFXManager : MonoBehaviour
    {
        private static SFXManager _instance;
        public static SFXManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SFXManager>();
                    if (_instance == null)
                    {
                        GameObject singletonGO = new GameObject("SFXManger_singleton");
                        _instance = singletonGO.AddComponent<SFXManager>();

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
        }

        public void PlaySFX(SFXEvent sfxEvent, GameObject soundOBJ)
        {
            if (sfxEvent.SpatialSound == 0)
            {
                soundOBJ.transform.parent = gameObject.transform;
            }
            soundOBJ.AddComponent<SFXPlayer>().Play(sfxEvent, soundOBJ);
        }

        public void PlayMultiSFX(MultiSFXEvent MsfxEvent, GameObject soundOBJ)
        {
            if (MsfxEvent.SpatialSound == 0)
            {
                soundOBJ.transform.parent = gameObject.transform;
            }
            soundOBJ.AddComponent<MultiSFXPlayer>().Play(MsfxEvent, soundOBJ);
        }
    }
}
