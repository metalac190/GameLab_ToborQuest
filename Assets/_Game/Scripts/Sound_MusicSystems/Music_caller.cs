using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class Music_caller : MonoBehaviour
{
    [SerializeField] MusicEvent MainSound;
    private void OnEnable()
    {
        MainSound.Play();
    }
}
