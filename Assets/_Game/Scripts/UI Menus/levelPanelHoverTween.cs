using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class levelPanelHoverTween : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float widthScale;
    public float heightScale;
    public float duration;
    public LeanTweenType expandTweenType = LeanTweenType.easeOutCubic;
    public LeanTweenType resetTweenType = LeanTweenType.easeOutSine;
    private RectTransform _transform;
    private Button _button;

    private void Awake()
    {
        _transform = this.GetComponent<RectTransform>();
        _button = this.GetComponent<Button>();
    }

    public void ExpandTween()
    {
        _transform.LeanScale(new Vector3(widthScale,heightScale, 1), duration).setEase(expandTweenType);
    }

    public void ResetTween()
    {
        _transform.LeanScale(new Vector3(1, 1, 1), duration).setEase(resetTweenType);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ExpandTween();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ResetTween();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //_button.onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExpandTween();
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetTween();
    }
}
