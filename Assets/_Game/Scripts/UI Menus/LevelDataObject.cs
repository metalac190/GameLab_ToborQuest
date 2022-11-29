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

    public float BestTimeSaved => PlayerPrefs.GetFloat(levelSaveTimeName);
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
	public void PrepData()
	{
        levelSaveTimeName = GetLevelSceneName() + "BestTime";

        bestTime = BestTimeSaved;
        BestTimeFormatted = TimerUI.ConvertTimeToText(bestTime);

	    SetNewMedal();
        SetNewGoal();
        NextGoalTimeFormatted = TimerUI.ConvertTimeToText(nextGoalTime);
    }

    public string GetLevelSceneName() => levelScene;

	public void SetNewBestTime(float time, string levelSaveTimeName)
	{
        bestTime = time;
        BestTimeFormatted = TimerUI.ConvertTimeToText(bestTime);
        PlayerPrefs.SetFloat(levelSaveTimeName, time);

	    SetNewMedal();
	    SetNewGoal();
    }
    
	public void SetNewMedal() => CurrentMedal = GetMedal(bestTime, BronzeGoal, SilverGoal, GoldGoal);
	
	public static MedalType GetMedal(float time, float bronze, float silver, float gold)
	{
		if (time == 0) return MedalType.None;
		float[] orderedGoalArray = new float[] { int.MaxValue, bronze, silver, gold };
		
		for(int i = 0; i < orderedGoalArray.Length; i++)
		{
			if(orderedGoalArray[i] >= time) continue;
			else return (MedalType)i - 1;
		}
		return MedalType.Gold;
	}

	public void SetNewGoal()
	{
	    NextGoalTimeFormatted = GetNextGoal(CurrentMedal, BronzeGoal, SilverGoal, GoldGoal);

        //set the new goal medal
        if((bestTime == 0) || (CurrentMedal == MedalType.Gold)) {
            NextGoalMedal = MedalType.None;
        } else {
            NextGoalMedal = CurrentMedal + 1;
        }
	}
	
	public static string GetNextGoal(MedalType medal, float bronze, float silver, float gold)
	{
		float nextBestTime = medal switch
		{
			MedalType.None => bronze,
			MedalType.Bronze => silver,
			MedalType.Silver => gold,
			MedalType.Gold => 0,
			_ => 0
		};
		return TimerUI.ConvertTimeToText(nextBestTime);
	}
}
