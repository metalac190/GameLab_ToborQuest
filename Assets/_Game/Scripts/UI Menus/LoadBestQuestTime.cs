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
    [SerializeField] private float _platinum;
	[SerializeField] private float _authorTime;
    [SerializeField] private Image _medalImage;
    [SerializeField] private Image _nextMedalImage;
    [SerializeField] private CanvasGroup _nextTimeSection;
    [SerializeField] private MedalUIHelper _medals;
    
	private void OnEnable()
	{
		ExtrasSettings.OnDataChanged += CheckQuestMedal;
	}
    
	private void OnDisable()
	{
		ExtrasSettings.OnDataChanged -= CheckQuestMedal;
	}
    
    private void Start()
	{
		CheckQuestMedal();
    }
    
	private void CheckQuestMedal()
	{
		float time = BestTimesSaver.GetBestTime(BestTime.Quest);
		var medal = MedalType.None;
		var nextMedal = MedalType.None;
		if (time > 0)
		{
			medal = LevelDataObject.GetMedal(time, _bronzeTime, _silverTime, _goldTime, _platinum, _authorTime);
			if (medal != MedalType.Author) nextMedal = medal + 1;
		}
		_nextTimeSection.alpha = time == 0 || medal == MedalType.Author ? 0 : 1;
		
		if (_medalImage) _medals.SetMedalUI(_medalImage, medal);
		if (_nextMedalImage) _medals.SetMedalUI(_nextMedalImage, nextMedal);
		_text.text = TimerUI.ConvertTimeToText(time);
		_nextBestText.text = LevelDataObject.GetNextGoalFormatted(medal, _bronzeTime, _silverTime, _goldTime, _platinum, _authorTime);
	}

    public void StartQuest()
    {
        CGSC.PlayingQuest = true;
        TimerUI.levelTime = 0;
        CGSC.TotalTime = 0;
        CGSC.LoadScene(_levelToLoadFirst, true, true);
    }
}
