using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatTimes : MonoBehaviour
{
	[Button]
	private void SetHighScores(float level1, float level2, float level3, float level4, float quest)
    {
	    PlayerPrefs.SetFloat("Level 1BestTime", level1);
	    PlayerPrefs.SetFloat("Level 2BestTime", level2);
	    PlayerPrefs.SetFloat("Level 3BestTime", level3);
	    PlayerPrefs.SetFloat("Level 4BestTime", level4);
	    PlayerPrefs.SetFloat(LoadBestQuestTime.QuestTimePref, quest);
	    ExtrasSettings.OnResetData?.Invoke();
    }
}
