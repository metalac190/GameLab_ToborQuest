using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    private Animator menuAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        menuAnimator = GetComponent<Animator>();
        LevelSelect(false);
    }

    //private void Update()
    //{
    //    Debug.Log("LevelSelect is " + menuAnimator.GetBool("LevelSelect"));
    //    Debug.Log("LevelInfoMenu is " + menuAnimator.GetBool("LevelInfoMenu"));
    //}

    public void QuestSelect(bool value, Action onComplete = null)
    {
        menuAnimator.SetBool("QuestSelect", value);
    }

    public void LevelSelect(bool value, Action onComplete = null)
    {
        menuAnimator.SetBool("LevelSelect", value);
        onComplete?.Invoke();
    }

    public void LevelInfoMenu(bool value, Action onComplete = null)
    {
        menuAnimator.SetBool("LevelInfoMenu", value);
        onComplete?.Invoke();
    }

    public void SettingsMenu(bool value, Action onComplete = null)
    {
        menuAnimator.SetBool("SettingsMenu", value);
        onComplete?.Invoke();
    }

	public void Credits(bool value, Action onComplete = null) {
		menuAnimator.SetBool("Credits", value);
		onComplete?.Invoke();
	}

    public Animator AnimatorController
    {
        get
        {
            return menuAnimator;
        }
    }

    public void StartLevelSelectMenu()
    {
        LevelSelect(true);
        QuestSelect(true);
        menuAnimator.Play("LevelSelect");
    }
}
