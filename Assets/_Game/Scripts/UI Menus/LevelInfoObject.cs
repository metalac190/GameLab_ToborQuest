using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class LevelInfoObject : ScriptableObject
{
    [SerializeField] private SerializedScene levelScene;
    public string levelInfo;
    public Sprite levelImage;
    public float BestTime { get; set; }
    public bool ghostDataAvailable { get; set; }
    private string levelSaveTimeName;
    public bool removeWhiteSpace = false;

    public string GetLevelSceneName()
    {
        return levelScene.Name;
    }

    public string GetTimeFormatted()
    {
        Debug.Log(removeWhiteSpace);
        if(removeWhiteSpace)
            levelSaveTimeName = levelScene.Name.Remove(5,1) + "BestTime";
        else
            levelSaveTimeName = levelScene.Name + "BestTime";

        BestTime = PlayerPrefs.GetFloat(levelSaveTimeName);
        Debug.Log(levelSaveTimeName + " " + BestTime);
        TimeSpan time = TimeSpan.FromSeconds(BestTime);
        if (BestTime > 3600)
            return time.ToString(@"hh\:mm\:ss\:fff");
        else
            return time.ToString(@"mm\:ss\:fff");
    }
}
