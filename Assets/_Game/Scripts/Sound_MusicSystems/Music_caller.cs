using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class Music_caller : MonoBehaviour
{
    [SerializeField] MusicEvent MainSound;

    // Start is called before the first frame update
    void Awake()
    {
        MainSound.Play();
    }
}
