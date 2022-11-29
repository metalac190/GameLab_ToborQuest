using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MedalType { None, Bronze, Silver, Gold, Author }

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
	[SerializeField] private float authorGoal;

	[SerializeField, ReadOnly] private float bestTime;
    [SerializeField, ReadOnly] private float nextGoalTime;
    [SerializeField, ReadOnly] private string levelSaveTimeName;

    public float BestTimeSaved => PlayerPrefs.GetFloat(levelSaveTimeName);
    public string LevelName => levelName;
	[ReadOnly] public string BestTimeFormatted;
	[ReadOnly] public MedalType CurrentMedal;
	[ReadOnly] public string NextGoalTimeFormatted;
	[ReadOnly] public MedalType NextGoalMedal;
	[ReadOnly] public bool ghostDataAvailable;
    public float BronzeGoal => bronzeGoal;
    public float SilverGoal => silverGoal;
    public float GoldGoal => goldGoal;
	public float AuthorGoal => authorGoal;

    //called the menu panel's awake so it gets prepped when launching menu
	public void PrepData()
	{
        levelSaveTimeName = GetLevelSceneName() + "BestTime";

        bestTime = BestTimeSaved;
		BestTimeFormatted = TimerUI.ConvertTimeToText(bestTime);
		UpdateMedalAndGoal();
    }

    public string GetLevelSceneName() => levelScene;

	public void SetNewBestTime(float time, string levelSaveTimeName)
	{
        bestTime = time;
        BestTimeFormatted = TimerUI.ConvertTimeToText(bestTime);
		PlayerPrefs.SetFloat(levelSaveTimeName, time);
		UpdateMedalAndGoal();
	}
    
	[Button]
	public void UpdateMedalAndGoal()
	{
		SetNewMedal();
		SetNewGoal();
	}
    
	public void SetNewMedal() => CurrentMedal = GetMedal(bestTime, BronzeGoal, SilverGoal, GoldGoal, AuthorGoal);
	
	public static MedalType GetMedal(float time, float bronze, float silver, float gold, float author)
	{
		if (time == 0) return MedalType.None;
		float[] orderedGoalArray = new float[] { int.MaxValue, bronze, silver, gold, author };
		
		for(int i = 0; i < orderedGoalArray.Length; i++)
		{
			if(orderedGoalArray[i] >= time) continue;
			else return (MedalType)i - 1;
		}
		return MedalType.Author;
	}

	public void SetNewGoal()
	{
		nextGoalTime = GetNextGoal(CurrentMedal, BronzeGoal, SilverGoal, GoldGoal, AuthorGoal);
		NextGoalTimeFormatted = TimerUI.ConvertTimeToText(nextGoalTime);

        //set the new goal medal
		if((bestTime == 0) || (CurrentMedal == MedalType.Author)) {
            NextGoalMedal = MedalType.None;
        } else {
            NextGoalMedal = CurrentMedal + 1;
        }
	}
	
	public static float GetNextGoal(MedalType medal, float bronze, float silver, float gold, float author)
	{
		var goalTime = medal switch
		{
			MedalType.None => bronze,
			MedalType.Bronze => silver,
			MedalType.Silver => gold,
			MedalType.Gold => author,
			MedalType.Author => 0,
			_ => 0
		};
		return goalTime;
	}
	
	public static string GetNextGoalFormatted(MedalType medal, float bronze, float silver, float gold, float author)
	{
		return TimerUI.ConvertTimeToText(GetNextGoal(medal, bronze, silver, gold, author));
	}
}
