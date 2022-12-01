using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MedalType { None, Bronze, Silver, Gold, Platinum, Author }

[CreateAssetMenu()]
public class LevelDataObject : ScriptableObject
{
    [SerializeField] private string levelScene;
    [SerializeField] private string levelName;
    [TextArea(3,6)] public string levelDecription;
    [SerializeField] private BestTime _levelSaveKey = BestTime.None;
    public Sprite levelPanelPreviewSprite; //preview image on level select page
    public Sprite levelTitleSprite;
    public Sprite levelNameSprite;
    public Sprite levelInfoPreviewSprite; //preview image on level info page
    public Sprite levelCompleteSprite;

    //a time at or below these values, gets the medal
    [SerializeField] private float bronzeGoal;
    [SerializeField] private float silverGoal;
    [SerializeField] private float goldGoal;
    [SerializeField] private float platinumGoal;
	[SerializeField] private float authorGoal;

	[SerializeField, ReadOnly] private float bestTime;
    [SerializeField, ReadOnly] private float nextGoalTime;
    public string LevelName => levelName;
	[ReadOnly] public string BestTimeFormatted;
	[ReadOnly] public MedalType CurrentMedal;
	[ReadOnly] public string NextGoalTimeFormatted;
	[ReadOnly] public MedalType NextGoalMedal;
	[ReadOnly] public bool ghostDataAvailable;
    public float BronzeGoal => bronzeGoal;
    public float SilverGoal => silverGoal;
    public float GoldGoal => goldGoal;
    public float PlatinumGoal => platinumGoal;
	public float AuthorGoal => authorGoal;

	public float BestTimeSaved => BestTimesSaver.GetBestTime(_levelSaveKey);

	public bool TrySetBestTime(float time)
	{
		if (!BestTimesSaver.TrySetBestTime(_levelSaveKey, time)) return false;
		CheckUpdateData();
		return true;
	}

	public void CheckUpdateData()
	{
		bestTime = BestTimeSaved;
		BestTimeFormatted = TimerUI.ConvertTimeToText(bestTime);
		UpdateMedalAndGoal();
    }

    public string GetLevelSceneName() => levelScene;

    [Button]
	public void UpdateMedalAndGoal()
	{
		SetNewMedal();
		SetNewGoal();
	}
    
	public void SetNewMedal() => CurrentMedal = GetMedal(bestTime, BronzeGoal, SilverGoal, GoldGoal, PlatinumGoal, AuthorGoal);
	
	public static MedalType GetMedal(float time, float bronze, float silver, float gold, float platinum, float author)
	{
		if (time == 0) return MedalType.None;
		float[] orderedGoalArray = new float[] { int.MaxValue, bronze, silver, gold, platinum, author };
		
		for(int i = 0; i < orderedGoalArray.Length; i++)
		{
			if(orderedGoalArray[i] >= time) continue;
			else return (MedalType)i - 1;
		}
		return MedalType.Author;
	}

	public void SetNewGoal()
	{
		nextGoalTime = GetNextGoal(CurrentMedal, BronzeGoal, SilverGoal, GoldGoal, PlatinumGoal, AuthorGoal);
		NextGoalTimeFormatted = TimerUI.ConvertTimeToText(nextGoalTime);

        //set the new goal medal
		if(bestTime == 0 || CurrentMedal == MedalType.Author) {
            NextGoalMedal = MedalType.None;
        } else {
            NextGoalMedal = CurrentMedal + 1;
        }
	}
	
	public static float GetNextGoal(MedalType medal, float bronze, float silver, float gold, float platinum, float author)
	{
		var goalTime = medal switch
		{
			MedalType.None => bronze,
			MedalType.Bronze => silver,
			MedalType.Silver => gold,
			MedalType.Gold => platinum,
			MedalType.Platinum => author,
			MedalType.Author => 0,
			_ => 0
		};
		return goalTime;
	}
	
	public static string GetNextGoalFormatted(MedalType medal, float bronze, float silver, float gold, float platinum, float author)
	{
		return TimerUI.ConvertTimeToText(GetNextGoal(medal, bronze, silver, gold, platinum, author));
	}
}
