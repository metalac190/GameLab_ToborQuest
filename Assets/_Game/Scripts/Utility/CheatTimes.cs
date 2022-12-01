using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatTimes : MonoBehaviour
{
	[Button]
	private void SetHighScores(float quest, float level1, float level2, float level3, float level4)
    {
	    BestTimesSaver.ForceSetBestTime(BestTime.Quest, quest);
	    BestTimesSaver.ForceSetBestTime(BestTime.Level1, level1);
	    BestTimesSaver.ForceSetBestTime(BestTime.Level2, level2);
	    BestTimesSaver.ForceSetBestTime(BestTime.Level3, level3);
	    BestTimesSaver.ForceSetBestTime(BestTime.Level4, level4);
	    ExtrasSettings.OnDataChanged?.Invoke();
    }
}
