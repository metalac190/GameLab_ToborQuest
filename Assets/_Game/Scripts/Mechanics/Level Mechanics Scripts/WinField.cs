using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public class WinField : InvisibleTrigger
{
	protected override void OnPlayerTrigger(MovementController player)
	{
		player.SetActive(false);
		player.GetComponent<Rigidbody>().isKinematic = true;
		CGSC.WinGame();
		playAudio();
	}

    #region winJingle Caller
    [SerializeField] SFXEvent _wingJingle;
	[SerializeField] SFXEvent _bSideWinJingle;

	private void playAudio()
	{
		if (SettingsSaver.BSideAudio == false)
		{
			_wingJingle.Play();
		}
		if (SettingsSaver.BSideAudio == true)
		{
			_bSideWinJingle.Play();
		}
	}
    #endregion
}
