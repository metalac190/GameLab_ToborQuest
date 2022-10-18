using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class levelPanelHoverTween : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public float widthScale;
    public float heightScale;
    public float duration;

    private RectTransform _transform;

    private void Awake()
    {
        _transform = this.GetComponent<RectTransform>();
    }

    public void ExpandTween()
    {
        _transform.LeanScale(new Vector3(widthScale,heightScale, 1), duration);
    }

    public void ResetScale()
    {
        _transform.LeanScale(new Vector3(1, 1, 1), duration);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ExpandTween();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ResetScale();
    }
}
