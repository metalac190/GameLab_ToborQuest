using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    [SerializeField] RectTransform _cachedOriginalTransform;
    Vector3 _hiddenPosition = new Vector3(840, 0, 0);
    Vector3 _visiblePosition = new Vector3(0, 0, 0);

    [SerializeField, Range(0.75f, 1)]
    private float dialogueScale;

    void Awake()
    {
        dialogueScale = CGSC.DialogueScale;
        Debug.Log(dialogueScale);
         _cachedOriginalTransform = GetComponent<RectTransform>();
        LeanTween.reset();
        //Set dialogue scale if playerPref or saved value exists
    }

    private void Update()
    {
        _cachedOriginalTransform.transform.localScale = new Vector3(dialogueScale, dialogueScale, 1);
    }

    public void IntroAnimation(float _time)
    {
        //Debug.Log("Intro");
        LeanTween.move(_cachedOriginalTransform, _visiblePosition, _time).setEase(LeanTweenType.easeInOutQuart);
        
        
    }


    public void ExitAnimation(float _time)
    {
        //Debug.Log("Exit");
        LeanTween.move(_cachedOriginalTransform, _hiddenPosition, _time).setEase(LeanTweenType.easeInOutQuart);

    }


    public void IntroAndExitAnimation(float enterTime, float timeWait, float exitTime)
    {
        StartCoroutine(Animation(enterTime, timeWait, exitTime));
    }

    public void CancelAnimations()
    {
        LeanTween.cancel(_cachedOriginalTransform);
    }

    IEnumerator Animation(float enterTime, float timeWait, float exitTime)
    {
        IntroAnimation(enterTime);
        yield return new WaitForSecondsRealtime(timeWait);
        Debug.Log("[Dialogue Animator] Exit");
        ExitAnimation(exitTime);
    }

}
