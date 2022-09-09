using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingManager : MonoBehaviour
{   
    public string saveFileName = "JonathanTestFile";
    public List<PersistableObject> persistableObjects = new List<PersistableObject>();

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
