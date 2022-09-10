using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SavingManager : PersistableObject
{
    const int saveVersion = 1;
    //public List<PersistableObject> persistableObjects = new List<PersistableObject>();
    public Slider saveSlider;
    public TMP_InputField saveInputField;
    public TextMeshProUGUI saveText;

    public PersistentStorage storage;

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
        //writer.Write(shapes.Count);
        //writer.Write(Random.state);
        //writer.Write(CreationSpeed);
        //writer.Write(creationProgress);
        //writer.Write(DestructionSpeed);
        //writer.Write(destructionProgress);
        //writer.Write(loadedLevelBuildIndex);
        //GameLevel.Current.Save(writer);

        //for (int i = 0; i < shapes.Count; i++)
        //{
        //    writer.Write(shapes[i].OriginFactory.FactoryId);
        //    writer.Write(shapes[i].ShapeId);
        //    writer.Write(shapes[i].MaterialId);
        //    shapes[i].Save(writer);
        //}
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
        saveInputField.text = "";
        //StartCoroutine(LoadGame(reader));
    }

    IEnumerator LoadGame(GameDataReader reader)
    {
        //int version = reader.Version;
        Debug.Log(reader.Version);
        //int count = version <= 0 ? -version : reader.ReadInt();

        //Version 1 we first read slider value then read string value.


        //if (version >= 3)
        //{
        //    Random.State state = reader.ReadRandomState();
        //    if (!reseedOnLoad)
        //    {
        //        Random.state = state;
        //    }
        //    creationSpeedSlider.value = CreationSpeed = reader.ReadFloat();
        //    creationProgress = reader.ReadFloat();
        //    destructionSpeedSlider.value = DestructionSpeed = reader.ReadFloat();
        //    destructionProgress = reader.ReadFloat();
        //}

        //yield return LoadLevel(version < 2 ? 1 : reader.ReadInt());
        //if (version >= 3)
        //{
        //    GameLevel.Current.Load(reader);
        //}

        //for (int i = 0; i < count; i++)
        //{
        //    int factoryId = version >= 5 ? reader.ReadInt() : 0;
        //    int shapeId = version > 0 ? reader.ReadInt() : 0;
        //    int materialId = version > 0 ? reader.ReadInt() : 0;
        //    Shape instance = shapeFactories[factoryId].Get(shapeId, materialId);
        //    instance.Load(reader);
        //}

        //for (int i = 0; i < shapes.Count; i++)
        //{
        //    shapes[i].ResolveShapeInstances();
        //}
        yield return new WaitForSeconds(1);
    }

}
