using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SavingManager : PersistableObject
{
    const int saveVersion = 1;

    public bool LoadOnAwake = true;

    public Slider saveSlider;
    public TMP_InputField saveInputField;
    public TextMeshProUGUI saveText;
    public TimerUI timer;

    public PersistentStorage storage;

    private void Awake()
    {
        if (LoadOnAwake)
            ButtonLoad();
    }

    public void ButtonSave()
    {
        storage.Save(this,saveVersion);
    }

    public void ButtonLoad()
    {
        storage.Load(this);
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(saveSlider.value);
        writer.Write(saveInputField.text);
        writer.Write(timer.timeRemaning);
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version > saveVersion)
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }
        saveSlider.value = reader.ReadFloat();
        saveText.text = reader.ReadString();       
        saveInputField.text = saveText.text;
        timer.timeRemaning = reader.ReadFloat();
        //StartCoroutine(LoadGame(reader));
    }

    //IEnumerator LoadGame(GameDataReader reader)
    //{
    //    //int version = reader.Version;
    //    Debug.Log(reader.Version);
        
    //    yield return new WaitForSeconds(1);
    //}

}
