using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLevelInfoPanel : MonoBehaviour
{
    //[SerializeField]
    //private string levelSaveName;
    [SerializeField]
    private TextMeshProUGUI bestTimeText;
    [SerializeField] private MedalUIHelper medalHelper;
    [SerializeField] private Image medalImage;
    [SerializeField]
    private TextMeshProUGUI nextGoalText;
    [SerializeField]
    private Image levelImage;

    [SerializeField]
    private LevelInfoObject levelInfoObj;

    float levelBestTime;

    void Start()
    {
        bestTimeText.text = levelInfoObj.GetBestTimeFormatted();
        medalHelper.SetMedalUI(medalImage, levelInfoObj.CurrentMedal);
        nextGoalText.text = levelInfoObj.GetNextTimeGoalFormatted();
        /*
        if (PlayerPrefs.HasKey(levelSaveName))
        {
            //Set best time text
            levelBestTime = PlayerPrefs.GetFloat(levelSaveName);
            TimeSpan time = TimeSpan.FromSeconds(levelBestTime);
            if (levelBestTime > 3600)
                bestTimeText.text = time.ToString(@"hh\:mm\:ss\:fff");
            else
                bestTimeText.text = time.ToString(@"mm\:ss\:fff");

            //Set the next goal text
            
            for (int i = 0; i < orderedGoalArray.Length; i++)
            {
                if (orderedGoalArray[i] < levelBestTime)
                {
                    TimeSpan tempTime = TimeSpan.FromSeconds(orderedGoalArray[i]);
                    if (levelBestTime > 3600)
                        nextGoalText.text = tempTime.ToString(@"hh\:mm\:ss\:fff");
                    else
                        nextGoalText.text = tempTime.ToString(@"mm\:ss\:fff");

                    break;
                }
            }            
        }
        else
        {
            bestTimeText.text = "--:--:---";
            nextGoalText.text = "--:--:---";
        }
        */
    }
}
