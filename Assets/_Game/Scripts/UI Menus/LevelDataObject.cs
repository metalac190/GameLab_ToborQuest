using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MedalType { None, Bronze, Silver, Gold }

[CreateAssetMenu()]
public class LevelDataObject : ScriptableObject
{
    [SerializeField] private string levelScene;
    [SerializeField] private string levelName;
    [TextArea(3,6)] public string levelDecription;
    public Sprite levelPanelPreviewSprite; //preview image on level select page
    public Sprite levelTitleSprite;
    public Sprite levelNameSprite;
    public Sprite levelInfoPreviewSprite; //preview image on level info page
    public Sprite levelCompleteSprite;

    //a time at or below these values, gets the medal
    [SerializeField] private float bronzeGoal;
    [SerializeField] private float silverGoal;
    [SerializeField] private float goldGoal;

    private float bestTime;
    private float nextGoalTime;
    private string levelSaveTimeName;

    public string LevelName => levelName;
    public string BestTimeFormatted { get; set; }
    public MedalType CurrentMedal { get; set; }
    public string NextGoalTimeFormatted { get; set; }
    public MedalType NextGoalMedal { get; set; }
    public bool ghostDataAvailable { get; set; }
    public float BronzeGoal => bronzeGoal;
    public float SilverGoal => silverGoal;
    public float GoldGoal => goldGoal;

    //called the menu panel's awake so it gets prepped when launching menu
    public void PrepData() {
        levelSaveTimeName = GetLevelSceneName() + "BestTime";

        bestTime = PlayerPrefs.GetFloat(levelSaveTimeName);
        BestTimeFormatted = SetFormattedTime(bestTime);

        SetNewGoal();
        NextGoalTimeFormatted = SetFormattedTime(nextGoalTime);

        ExtrasSettings.OnResetData += ResetData;
    }

    public string GetLevelSceneName() => levelScene;

    public void SetNewBestTime(float time, string levelSaveTimeName) {
        //set the new best time
        bestTime = time;
        BestTimeFormatted = SetFormattedTime(bestTime);
        PlayerPrefs.SetFloat(levelSaveTimeName, time);

        //set the new medal
        float[] orderedGoalArray = new float[] { int.MaxValue, BronzeGoal, SilverGoal, GoldGoal };

        for(int i = 0; i < orderedGoalArray.Length; i++) {
            if(orderedGoalArray[i] >= bestTime) {
                continue;
            } else {
                CurrentMedal = (MedalType)i - 1;
                return;
            }
        }
        CurrentMedal = MedalType.Gold;

        //set the new goal
        SetNewGoal();
    }

    public void SetNewGoal() {
        float[] orderedGoalArray = new float[] { BronzeGoal, SilverGoal, GoldGoal };

        //set the new goal time
        nextGoalTime = 0; //only stays zero if the best time is gold
        for(int i = 0; i < orderedGoalArray.Length; i++) {
            if(orderedGoalArray[i] < bestTime) {
                nextGoalTime = orderedGoalArray[i];
                break;
            }
        }
        NextGoalTimeFormatted = SetFormattedTime(nextGoalTime);

        //set the new goal medal
        if((bestTime == 0) || (CurrentMedal == MedalType.Gold)) {
            NextGoalMedal = MedalType.None;
        } else {
            NextGoalMedal = CurrentMedal + 1;
        }
    }

    private string SetFormattedTime(float time) {
        TimeSpan tempTime = TimeSpan.FromSeconds(time);
        if(time == 0)
            return "--:--:---";
        else if(time > 3600)
            return tempTime.ToString(@"hh\:mm\:ss\:fff");
        else
            return tempTime.ToString(@"mm\:ss\:fff");
    }

    private void ResetData() {
        bestTime = 0;
        CurrentMedal = MedalType.None;
        BestTimeFormatted = SetFormattedTime(bestTime);
        nextGoalTime = 0;
        NextGoalMedal = MedalType.None;
        NextGoalTimeFormatted = SetFormattedTime(nextGoalTime);
    }
}
