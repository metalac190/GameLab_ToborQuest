using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MedalType { None, Bronze, Silver, Gold }

[CreateAssetMenu()]
public class LevelInfoObject : ScriptableObject
{
    [SerializeField] private SerializedScene levelScene;
    [TextArea(3,6)] public string levelDecription;
    public Sprite levelPanelPreviewSprite; //preview image on level select page
    public Sprite levelTitleSprite;
    public Sprite levelNameSprite;
    public Sprite levelInfoPreviewSprite; //preview image on level info page

    //a time at or below these values, gets the medal
    [SerializeField] private float bronzeGoal;
    [SerializeField] private float silverGoal;
    [SerializeField] private float goldGoal;


    public float BestTime { get; set; }
    public MedalType CurrentMedal { get; set; }
    public bool ghostDataAvailable { get; set; }
    public float BronzeGoal => bronzeGoal;
    public float SilverGoal => silverGoal;
    public float GoldGoal => goldGoal;

    private void Awake()
    {
        levelScene.CheckValid();
    }


    public string GetLevelSceneName()
    {
        levelScene.CheckValid();
        return levelScene.Name;
    }

    public string GetBestTimeFormatted()
    {
        string levelSaveTimeName = levelScene.Name + "BestTime";
        if (PlayerPrefs.HasKey(levelSaveTimeName))
        {
            BestTime = PlayerPrefs.GetFloat(levelSaveTimeName);
            SetCurrentMedal();
            TimeSpan time = TimeSpan.FromSeconds(BestTime);
            if (BestTime > 3600)
                return time.ToString(@"hh\:mm\:ss\:fff");
            else
                return time.ToString(@"mm\:ss\:fff");
        }
        else
        {
            return "--:--:---";
        }        
    }

    public string GetNextTimeGoalFormatted()
    {
        float[] orderedGoalArray = new float[] { BronzeGoal, SilverGoal, GoldGoal };

        for(int i = 0; i < orderedGoalArray.Length; i++)
        {
            if(orderedGoalArray[i] < BestTime)
            {
                TimeSpan tempTime = TimeSpan.FromSeconds(orderedGoalArray[i]);
                if(BestTime > 3600)
                    return tempTime.ToString(@"hh\:mm\:ss\:fff");
                else
                    return tempTime.ToString(@"mm\:ss\:fff");
            }
        }
        return "--:--:---";
    }

    private void SetCurrentMedal()
    {
        float[] orderedGoalArray = new float[] { int.MaxValue, BronzeGoal, SilverGoal, GoldGoal };

        for(int i = 0; i < orderedGoalArray.Length; i++) {
            if(orderedGoalArray[i] >= BestTime)
            {
                continue;
            }
            else
            {
                CurrentMedal = (MedalType)i - 1;
                return;
            }
        }
        CurrentMedal = MedalType.Gold;
    }

    public MedalType GetNextMedalGoal()
    {
        if(BestTime == 0) return MedalType.None;
        if(CurrentMedal == MedalType.Gold) return MedalType.None;
        return CurrentMedal + 1;
    }
}
