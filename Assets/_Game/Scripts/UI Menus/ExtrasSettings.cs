using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasSettings : MonoBehaviour
{
    public static bool DialogueDisabled;
    public static Action OnResetData = delegate { };
    
    [SerializeField] private GameObject _disableDialogueActive;
    
    public void ResetData()
    {
        // TODO: Make this only clear times once audio and visual settings are saved
        PlayerPrefs.DeleteAll();
        OnResetData?.Invoke();
    }

    public void ToggleDialogue()
    {
        DialogueDisabled = !DialogueDisabled;
        _disableDialogueActive.SetActive(DialogueDisabled);
    }
}
