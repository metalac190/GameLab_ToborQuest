using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuArrowTween : MonoBehaviour
{
    public int bouncingDistance = 50;
    public float bouncingTime = 1.5f;    
    
    private int tweenID;
    private RectTransform _transform;
    private Vector3 initPos;

    private void Awake()
    {
        _transform = gameObject.GetComponent<RectTransform>();
        initPos = _transform.localPosition;
        LeanTween.reset();
        if(EventSystem.current.currentSelectedGameObject != transform.parent.gameObject)
        {
            gameObject.SetActive(false);            
        }
        else
        {
            gameObject.SetActive(true);
        }            
    }

    private void OnEnable()
    {
        _transform.localPosition = initPos;
        ArrowTween();
    }

    void ArrowTween()
    {
        LeanTween.cancel(_transform);
        tweenID = LeanTween.moveX(_transform, bouncingDistance, bouncingTime)
            .setEaseOutCubic()
            .setLoopPingPong()
            .setIgnoreTimeScale(true).id;
    }

    void PauseTween()
    {
        LeanTween.pause(tweenID);
    }

    void ResumeTween()
    {
        LeanTween.resume(tweenID);
    }

}
