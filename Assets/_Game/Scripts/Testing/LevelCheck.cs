using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCheck : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdownQuest;
    [SerializeField] private TMP_Dropdown _dropdownLevels;

    private void Start()
    {
        foreach (var quest in CGSC.QuestNames)
        {
            _dropdownQuest.options.Add(new TMP_Dropdown.OptionData(quest));
        }
    }

    public void SelectQuest(int option)
    {
        _dropdownLevels.ClearOptions();
        _dropdownLevels.options.Add(new TMP_Dropdown.OptionData("None"));
        _dropdownLevels.SetValueWithoutNotify(0);
        
        if (option == 0) return;
        var selectedQuest = CGSC.Quests[option - 1];
        
        foreach (var level in selectedQuest.LevelNames)
        {
            _dropdownLevels.options.Add(new TMP_Dropdown.OptionData(level));
        }
    }
}
