using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadBestQuestTime : MonoBehaviour
{
    [SerializeField] private string _levelToLoadFirst = "Level 1";
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _nextBestText;
    [SerializeField] private float _bronzeTime;
    [SerializeField] private float _silverTime;
    [SerializeField] private float _goldTime;
    [SerializeField] private Image _medalImage;
    [SerializeField] private MedalUIHelper _medals;
    
	public const string QuestTimePref = "BestQuestTime";
    
	private void OnEnable()
	{
		ExtrasSettings.OnResetData += CheckQuestMedal;
	}
    
	private void OnDisable()
	{
		ExtrasSettings.OnResetData -= CheckQuestMedal;
	}
    
    private void Start()
	{
		CheckQuestMedal();
    }
    
	private void CheckQuestMedal()
	{
		float time = 0;
		var medal = MedalType.Bronze;
		if (PlayerPrefs.HasKey(QuestTimePref))
		{
			time = PlayerPrefs.GetFloat(QuestTimePref);
			medal = LevelDataObject.GetMedal(time, _bronzeTime, _silverTime, _goldTime);
		}
		_medals.SetMedalUI(_medalImage, medal);
		_text.text = TimerUI.ConvertTimeToText(time);
		_nextBestText.text = LevelDataObject.GetNextGoal(medal, _bronzeTime, _silverTime, _goldTime);
	}

    private MedalType GetMedal(float time)
    {
        float[] orderedGoalArray = new float[] { int.MaxValue, _bronzeTime, _silverTime, _goldTime };

        for(int i = 0; i < orderedGoalArray.Length; i++)
        {
            if(orderedGoalArray[i] >= time) continue;
            return (MedalType)i - 1;
        }
        return MedalType.Gold;
    }

    public void StartQuest()
    {
        CGSC.PlayingQuest = true;
        TimerUI.levelTime = 0;
        CGSC.TotalTime = 0;
        CGSC.LoadScene(_levelToLoadFirst, true, true);
    }
}
