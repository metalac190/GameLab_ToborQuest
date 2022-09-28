using UnityEngine;

[System.Serializable]
public class SerializedScene
{
    public Object Scene;
    public string Name;
    public bool Exists;
    public bool Valid;
    
    public void CheckValid(bool debugIfNotValid = false)
    {
        Exists = Scene != null;
        Name = Exists ? Scene.name : "";
        Valid = Exists && Application.CanStreamedLevelBeLoaded(Name);
        if (debugIfNotValid && Exists && !Valid)
        {
            Debug.LogError($"Scene ({Name}) Is Not In Build Settings!");
        }
    }
}