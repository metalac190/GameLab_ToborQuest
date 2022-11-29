using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasSettings : MonoBehaviour
{
    public static Action OnResetData = delegate { };
    
	[SerializeField] private GameObject _bSideAudioActive;
	[SerializeField] private GameObject _disableDialogueActive;
    
	public static bool DialogueDisabled => SavingManager.DisableDialogue;
	public static bool BSideAudio => SavingManager.BSideAudio;
    
    private void Start()
    {
        LoadValues();
    }

    private void LoadValues()
	{
		_disableDialogueActive.SetActive(DialogueDisabled);
		_bSideAudioActive.SetActive(BSideAudio);
    }
    
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        CGSC.SaveSystem.DeleteSave();
        CGSC.SaveSystem.LoadDefaults();
        OnResetData?.Invoke();
        LoadValues();
    }

    public void ToggleDialogue()
    {
        SavingManager.DisableDialogue = !DialogueDisabled;
        _disableDialogueActive.SetActive(DialogueDisabled);
    }

	public void ToggleBSideAudio()
	{
		SavingManager.BSideAudio = !BSideAudio;
		_bSideAudioActive.SetActive(BSideAudio);
	}
}
