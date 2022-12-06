using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasSettings : MonoBehaviour
{
    public static Action OnDataChanged = delegate { };
	public static Action OnChangeBSide = delegate { };
    
	[SerializeField] private GameObject _bSideAudioActive;
	[SerializeField] private GameObject _disableDialogueActive;
    
	public static bool DialogueDisabled => SettingsSaver.DisableDialogue;
	public static bool BSideAudio => SettingsSaver.BSideAudio;
    
    private void Start()
    {
        LoadValues();
    }

    private void LoadValues()
	{
		_disableDialogueActive.SetActive(DialogueDisabled);
		_bSideAudioActive.SetActive(BSideAudio);
    }
    
	public void ResetAllSettings()
    {
        PlayerPrefs.DeleteAll();
        CGSC.SettingsSaver.DeleteSave();
        OnDataChanged?.Invoke();
        LoadValues();
    }
    
	public void ResetAllTimes()
	{
		CGSC.BestTimesSaver.DeleteSave();
		OnDataChanged?.Invoke();
	}

    public void ToggleDialogue()
    {
        SettingsSaver.DisableDialogue = !DialogueDisabled;
        _disableDialogueActive.SetActive(DialogueDisabled);
    }

	public void ToggleBSideAudio()
	{
		SettingsSaver.BSideAudio = !BSideAudio;
		_bSideAudioActive.SetActive(BSideAudio);
		OnChangeBSide?.Invoke();
	}
}
