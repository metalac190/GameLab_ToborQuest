using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinField : InvisibleTrigger
{
	protected override void OnPlayerTrigger(Collider other)
	{
		base.OnPlayerTrigger(other);
		var player = other.GetComponent<MovementController>();
		if (player)
		{
			player.SetActive(false);
			CGSC.WinGame();    
		}
	}
   

    //public void OnWin()
    //{
            
    //}
}
