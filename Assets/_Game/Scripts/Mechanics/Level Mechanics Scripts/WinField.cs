using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinField : InvisibleTrigger
{
	protected override void OnPlayerTrigger(MovementController player)
	{
		player.SetActive(false);
		player.GetComponent<Rigidbody>().isKinematic = true;
		CGSC.WinGame();
	}
}
