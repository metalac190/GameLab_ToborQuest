using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{ 
    public class SFXManager : MonoBehaviour
    {
        GameObject[] _poolingObjects;
        int _poolOBJtoUse;

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
            settingUpPoolObjects();
        }

        public void PlaySFX(SFXEvent sfxEvent)
        {
            if (sfxEvent.SpatialSound == 0)
            {
                _poolingObjects[_poolOBJtoUse].transform.parent = gameObject.transform;
            }
            _poolingObjects[_poolOBJtoUse].GetComponent<SFXPlayer>().Play(sfxEvent);
            _poolOBJtoUse += 1;
            if (_poolOBJtoUse >= 20)
            {
                _poolOBJtoUse = 0;
            }
        }

        public void PlayMultiSFX(MultiSFXEvent MsfxEvent, GameObject soundOBJ)
        {
            if (MsfxEvent.SpatialSound == 0)
            {
                soundOBJ.transform.parent = gameObject.transform;
            }
            soundOBJ.AddComponent<MultiSFXPlayer>().Play(MsfxEvent, soundOBJ);
        }

        private void settingUpPoolObjects()
        {
            _poolOBJtoUse = 0;
            _poolingObjects = new GameObject[20];

            for (int i = 0; i < _poolingObjects.Length; i++)
            {
                _poolingObjects[i] = new GameObject("poolObject_" + i);
                _poolingObjects[i].transform.parent = this.gameObject.transform;
                _poolingObjects[i].AddComponent<AudioSource>();
                _poolingObjects[i].AddComponent<SFXPlayer>();
            }
        }
    }
}
