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

    private void Awake()
    {
        LeanTween.reset();
    }

    private void Start()
    {
        ArrowTween();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("Pause key");
            PauseTween();
        }
        else if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("Resume key");
            ResumeTween();
        }
    }

    void ArrowTween()
    {
        LeanTween.cancel(gameObject.GetComponent<RectTransform>());
        tweenID = LeanTween.moveX(gameObject.GetComponent<RectTransform>(), bouncingDistance, bouncingTime)
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
