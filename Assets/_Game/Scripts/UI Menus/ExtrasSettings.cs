using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasSettings : MonoBehaviour
{
    public static bool DialogueDisabled;
    public static Action OnResetData = delegate { };
    
    [SerializeField] private GameObject _disableDialogueActive;
    
    private void Start()
    {
        LoadValues();
    }

    private static void LoadValues()
    {
        DialogueDisabled = SavingManager.DisableDialogue;
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
        DialogueDisabled = !DialogueDisabled;
        _disableDialogueActive.SetActive(DialogueDisabled);
    }

    public void SetDialogueActiveImage(bool value)
    {
        _disableDialogueActive.SetActive(value);
    }

}
