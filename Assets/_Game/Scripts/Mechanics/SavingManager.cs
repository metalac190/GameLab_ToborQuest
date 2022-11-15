using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SavingManager : PersistableObject
{
    const int saveVersion = 1;

    public bool LoadOnAwake = true;

    public Action onSaveSettings = delegate { };
    public Action onLoadSettings = delegate { };

    public PersistentStorage storage;

    [Header("Audio")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider dialogueSlider;
    public Slider ambienceSlider;
    [Header("Visuals")]
    [ReadOnly]private string visualQuality="High";
    public Button highQButton, mediumQButton, lowQButton;
    [Header("Extra")]
    public ExtrasSettings extraSettings;
    public Slider dialogueScale;

    private void Awake()
    {
        if (LoadOnAwake)
            ButtonLoad();
    }

    public void ButtonSave()
    {
        storage.Save(this,saveVersion);
        //onSaveSettings.Invoke();
    }

    public void ButtonLoad()
    {
        storage.Load(this);
        //onLoadSettings.Invoke();
    }

    public void DeleteSave()
    {
        storage.DeleteStorage();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(masterSlider.value);
        writer.Write(musicSlider.value);
        writer.Write(sfxSlider.value);
        writer.Write(dialogueSlider.value);
        writer.Write(ambienceSlider.value);
        writer.Write(dialogueScale.value);
        writer.Write(visualQuality);
        writer.Write(ExtrasSettings.DialogueDisabled);
        //writer.Write(saveSlider.value);
        //writer.Write(saveInputField.text);
        //writer.Write(timer.timeRemaining);
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version > saveVersion)
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }
        masterSlider.value = reader.ReadFloat();
        musicSlider.value = reader.ReadFloat();
        sfxSlider.value = reader.ReadFloat();
        dialogueSlider.value = reader.ReadFloat();
        ambienceSlider.value = reader.ReadFloat();
        dialogueScale.value = reader.ReadFloat();
        string visualText = reader.ReadString();
        switch (visualText)
        {
            case "High":
                highQButton.onClick.Invoke();
                //EventSystem.current.SetSelectedGameObject(highQButton.gameObject);
                break;
            case "Medium":
                mediumQButton.onClick.Invoke();
                //EventSystem.current.SetSelectedGameObject(mediumQButton.gameObject);
                break;
            case "Low":
                lowQButton.onClick.Invoke();
                //EventSystem.current.SetSelectedGameObject(lowQButton.gameObject);
                break;
        }

        bool dialogueBool = reader.ReadBool();
        ExtrasSettings.DialogueDisabled = dialogueBool;
        extraSettings.SetDialogueActiveImage(dialogueBool);
        //saveSlider.value = reader.ReadFloat();
        //saveText.text = reader.ReadString();       
        //saveInputField.text = saveText.text;
        //timer.timeRemaining = reader.ReadFloat();
        //StartCoroutine(LoadGame(reader));
    }

    public void SetVisual(string value)
    {
        visualQuality = value;
    }

    //IEnumerator LoadGame(GameDataReader reader)
    //{
    //    //int version = reader.Version;
    //    Debug.Log(reader.Version);
        
    //    yield return new WaitForSeconds(1);
    //}

}
