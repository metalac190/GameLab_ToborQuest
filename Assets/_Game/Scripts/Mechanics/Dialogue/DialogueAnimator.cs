using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    [SerializeField] RectTransform _cachedOriginalTransform;
    [SerializeField] Vector3 _hiddenPosition;
    [SerializeField] Vector3 _visiblePosition;
    [SerializeField] bool _runOnStart = false;

    void OnValidate()
    {
        if (_cachedOriginalTransform == null) { _cachedOriginalTransform = GetComponent<RectTransform>(); }
    }

    void Start()
    {
        if (_runOnStart)
        {
            ExitAnimation(0.5f);
        }
    }

    [Button]
    public void IntroAnimation(float _time)
    {
        // LeanTween.move(_cachedOriginalTransform, _cachedFinalTransform, _time).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(_cachedOriginalTransform, _visiblePosition, _time).setEase(LeanTweenType.easeInOutQuart);

    }

    [Button]
    public void ExitAnimation(float _time)
    {
        // LeanTween.move(_cachedOriginalTransform, _startTransform, _time).setEase(LeanTweenType.easeInOutQuart);
        LeanTween.move(_cachedOriginalTransform, _hiddenPosition, _time).setEase(LeanTweenType.easeInOutQuart);


    }

    [Button]
    public void IntroAndExitAnimation(float enterTime, float timeWait, float exitTime)
    {
        StartCoroutine(Animation(enterTime, timeWait, exitTime));
    }

    IEnumerator Animation(float enterTime, float timeWait, float exitTime)
    {
        IntroAnimation(enterTime);
        yield return new WaitForSeconds(timeWait);
        ExitAnimation(exitTime);

        
    }

}
