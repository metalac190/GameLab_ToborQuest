using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    private Animator menuAnimator;

    // Start is called before the first frame update
    void Start()
    {
        menuAnimator = GetComponent<Animator>();
        LevelSelect(false);
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

    public Animator AnimatorController
    {
        get
        {
            return menuAnimator;
        }
    }
}
