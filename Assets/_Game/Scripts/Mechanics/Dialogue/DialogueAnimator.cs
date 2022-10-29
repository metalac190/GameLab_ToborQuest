using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    [SerializeField] RectTransform _cachedOriginalTransform;
    [SerializeField] Vector3 _hiddenPosition;
    [SerializeField] Vector3 _visiblePosition;

    

    void Start()
    {
     
         _cachedOriginalTransform = GetComponent<RectTransform>();
    }

    [Button]
    public void IntroAnimation(float _time = 1f)
    {
        Debug.Log("[Dialogue Animator] Intro");

        LeanTween.move(_cachedOriginalTransform, _visiblePosition, _time).setEase(LeanTweenType.easeInOutQuart);



    }

    [Button]
    public void ExitAnimation(float _time = 1f)
    {
       
        LeanTween.move(_cachedOriginalTransform, _hiddenPosition, _time).setEase(LeanTweenType.easeInOutQuart);
        Debug.Log("[Dialogue Animator] Exit");




    }

    [Button]
    public void IntroAndExitAnimation(float enterTime = 1f, float timeWait = 3f, float exitTime = 1f)
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
        yield return new WaitForSeconds(timeWait);
        Debug.Log("[Dialogue Animator] Exit");
        ExitAnimation(exitTime);
    }

}
