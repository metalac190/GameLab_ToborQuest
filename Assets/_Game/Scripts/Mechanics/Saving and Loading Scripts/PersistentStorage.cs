using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    public string _saveFileName = "SettingsValues";
    
    private string SavePath => Path.Combine(Application.persistentDataPath, _saveFileName);

    public void Save(PersistableObject o, int version)
    {
        using var writer = new BinaryWriter(File.Open(SavePath, FileMode.Create));
        writer.Write(-version);
        o.Save(new GameDataWriter(writer));
    }

    public bool Load(PersistableObject o)
    {
        if (!File.Exists(SavePath)) return false;
        byte[] data = File.ReadAllBytes(SavePath);
        var reader = new BinaryReader(new MemoryStream(data));
        o.Load(new GameDataReader(reader, -reader.ReadInt32()));
        return true;
    }

    public void DeleteStorage()
    {
        try
        {
            File.Delete(SavePath);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }
    }
}
