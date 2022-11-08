using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class AutoScrollRectPosition : MonoBehaviour
{
    //This variable will be used to specify the position the content rect should be.
    public Dictionary<HeaderNames.Header, int> headerHeightPos = new Dictionary<HeaderNames.Header, int>() {
        {HeaderNames.Header.Audio, 0 },
        {HeaderNames.Header.Visual, 765 },
        {HeaderNames.Header.Extra, 765 }
    };
    [SerializeField]
    Scrollbar _scrollbar;
    private float startValue;
    private float targetValue;
    //private RectTransforms
    //HeaderNames.Header lastHeaderName = HeaderNames.Header.Audio;
    // Start is called before the first frame update
    void Awake()
    {
        SetScrollbarValue(1f);
    }

    private void Update()
    {
        //_scrollbar.value = Mathf.Lerp(startValue, targetValue, Time.time);
    }

    public void SetScrollbarValue(float value)
    {
        if (value > 1 && value < 0)
        {
            Debug.LogWarning("Scrollbar only takes values from 0 to 1");
            return;
        }
        //targetValue = value;
        //startValue = _scrollbar.value;
        _scrollbar.value = value;
    }

    //public void SetPositionFromParent(GameObject obj)
    //{
    //    //HeaderNames.Header value = obj.GetComponent<HeaderNames>().headerClass;
    //    //Debug.Log("Current header " + value + ", previous header " + lastHeaderName);
    //    ////If same header class than previous object then don't move.
    //    //if (value == lastHeaderName)
    //    //    return;
    //    ////On change of selected gameobject we check
    //    //Debug.Log("Header has changed");
    //    //switch (value)
    //    //{
    //    //    case (HeaderNames.Header.Audio):
    //    //        SetContentPosition(headerHeightPos[value],value);
    //    //        break;
    //    //    case (HeaderNames.Header.Visual):
    //    //        SetContentPosition(headerHeightPos[value], value);
    //    //        break;
    //    //    case (HeaderNames.Header.Extra):
    //    //        SetContentPosition(headerHeightPos[value], value);
    //    //        break;
    //    //}
        
    //}

    //private void SetContentPosition(int value)
    //{
    //    //Debug.Log("Setting content value to " + value);
    //    //float scrollRectY = Mathf.Clamp(value, 0, 765);
    //    //contentTransform.localPosition = new Vector2(contentTransform.localPosition.x, scrollRectY);
    //    //lastHeaderName = headerName;
    //}





}
