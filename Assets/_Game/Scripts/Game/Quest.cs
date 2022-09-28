using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu]
public class Quest : ScriptableObject
{
    [SerializeField] private string _questName;
    [SerializeField] private List<SerializedScene> _questLevels;
    [SerializeField] private List<LevelMetals> _levelMetals;

    public List<SerializedScene> ValidLevels => _questLevels.Where(quest => quest.Valid).ToList();
    public List<string> LevelNames => (from level in ValidLevels select level.Name).ToList();

    public string GetLevelName(int index) => ValidLevels[index].Name;
    public void LoadLevel(int index) => CGSC.LoadScene(GetLevelName(index));
    
    public void OnValidate()
    {
        CheckLevelsValid();
    }

    public void CheckLevelsValid()
    {
        foreach (var level in _questLevels)
        {
            level.CheckValid(true);
        }
    }
}
