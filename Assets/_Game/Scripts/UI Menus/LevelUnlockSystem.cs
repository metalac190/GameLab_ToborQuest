using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockSystem : MonoBehaviour
{
    [SerializeField] private LevelDataObject _level1Data;
    [SerializeField] private CanvasGroup _level1;
    [SerializeField] private LevelDataObject _level2Data;
    [SerializeField] private CanvasGroup _level2;
    [SerializeField] private LevelDataObject _level3Data;
    [SerializeField] private CanvasGroup _level3;
    [SerializeField] private LevelDataObject _level4Data;
    [SerializeField] private CanvasGroup _level4;
    
    private void Start()
    {
        UpdateUnlocks();
    }

	public void UpdateUnlocks()
    {
	    SetGroupActive(_level1, _level1Data.BestTimeSaved > 0);
	    SetGroupActive(_level2, _level2Data.BestTimeSaved > 0);
	    SetGroupActive(_level3, _level3Data.BestTimeSaved > 0);
	    SetGroupActive(_level4, _level4Data.BestTimeSaved > 0);
    }

    private static void SetGroupActive(CanvasGroup group, bool active)
    {
        group.alpha = active ? 1 : 0.25f;
        group.interactable = active;
        group.blocksRaycasts = active;
    }
}
