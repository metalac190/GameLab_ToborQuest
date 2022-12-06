using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class Music_caller : MonoBehaviour
{
    [SerializeField] MusicEvent MainSound;
	[SerializeField] MusicEvent BTracks;
	
	private bool _playingBSide;
	
    
    private void OnEnable()
	{
		if (ExtrasSettings.BSideAudio && BTracks != null)
		{
			_playingBSide = true;
			BTracks.Play();
			return;
		}
		_playingBSide = false;
		MainSound.Play();
		
		ExtrasSettings.OnChangeBSide += CheckPlayAudio;
	}
	
	private void OnDisable()
	{
		ExtrasSettings.OnChangeBSide -= CheckPlayAudio;
	}
	
	private void CheckPlayAudio()
	{
		if (ExtrasSettings.BSideAudio && !_playingBSide && BTracks != null)
		{
			_playingBSide = true;
			BTracks.Play();
		}
		else if (!ExtrasSettings.BSideAudio && _playingBSide)
		{
			_playingBSide = false;
			MainSound.Play();
		}
	}
}
