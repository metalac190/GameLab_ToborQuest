using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class Music_caller : MonoBehaviour
{
    [SerializeField] MusicEvent MainSound;
	[SerializeField] MusicEvent BTracks;
    
    private void OnEnable()
	{
		if (ExtrasSettings.BSideAudio && BTracks != null)
		{
			BTracks.Play();
			return;
		}
		MainSound.Play();
    }
}
