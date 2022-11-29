using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class TimerStartTrigger : InvisibleTrigger
{
    private HUDManager _hud;
    
    
    private void Awake()
    {
        _hud = FindObjectOfType<HUDManager>();
    }
    
	protected override void OnPlayerTrigger(MovementController player)
    {
        _hud.StartTimer();
        if (MusicManager.Instance.ActiveLayerIndex == 0)
            MusicManager.Instance.EndDrums();
    }
}
