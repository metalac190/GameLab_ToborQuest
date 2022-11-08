using UnityEngine;

[System.Serializable]
public class SerializedScene
{
    public Object Scene;
    public string Name;
    public bool Valid;
    
    public void CheckValid(bool debugIfNotValid = false)
    {
        Valid = Scene != null;
        Name = Valid ? Scene.name : "";
    }
}