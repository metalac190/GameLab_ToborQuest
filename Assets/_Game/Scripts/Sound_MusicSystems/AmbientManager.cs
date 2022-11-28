using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public class AmbientManager : MonoBehaviour
    {
        GameObject[] _poolingObjects;
        int _poolOBJtoUse;

        private static AmbientManager _instance;
        public static AmbientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AmbientManager>();
                    if (_instance == null)
                    {
                        GameObject singletonGO = new GameObject("AMBManger_singleton");
                        _instance = singletonGO.AddComponent<AmbientManager>();

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

        public void PlayAMB(AmbientEvent ambEvent, GameObject parent)
        {
            _poolingObjects[_poolOBJtoUse].transform.position = parent.transform.position;
            _poolingObjects[_poolOBJtoUse].GetComponent<AmbientPlayer>().Play(ambEvent);
            _poolOBJtoUse += 1;
            if (_poolOBJtoUse >= 10)
            {
                _poolOBJtoUse = 0;
            }
        }

        private void settingUpPoolObjects()
        {
            _poolOBJtoUse = 0;
            _poolingObjects = new GameObject[10];

            for (int i = 0; i < _poolingObjects.Length; i++)
            {
                _poolingObjects[i] = new GameObject("ambientPoolObject_" + i);
                _poolingObjects[i].transform.parent = this.gameObject.transform;
                _poolingObjects[i].AddComponent<AudioSource>();
                _poolingObjects[i].AddComponent<AmbientPlayer>();
            }
        }
    }
}
