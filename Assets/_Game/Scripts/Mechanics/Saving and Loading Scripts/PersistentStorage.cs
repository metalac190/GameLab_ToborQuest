using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    public string saveFileName = "Placeholder";
    string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }

    public void Save(PersistableObject o, int version)
    {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        )
        {
            writer.Write(-version);
            o.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistableObject o)
    {
        if (File.Exists(savePath))
        {
            byte[] data = File.ReadAllBytes(savePath);
            var reader = new BinaryReader(new MemoryStream(data));
            o.Load(new GameDataReader(reader, -reader.ReadInt32()));
        }
        else
        {
            Debug.Log("First savefile creating");
        }
    }

    public void DeleteStorage()
    {
        try
        {
            File.Delete(savePath);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }
    }
}
