using System;
using System.Collections;
using System.Collections.Generic;
using SoundSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class levelPanelHoverTween : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _enableOnSelected;
    [SerializeField] private GameObject _hideOnSelected;
    [SerializeField] private MenuArrowTween _tweenToEnableOnSelected;
    [SerializeField] private SFXEvent _sfxOnSelectHover;
    [SerializeField] private SFXEvent _sfxOnClick;
    [SerializeField] private bool _animateScale = true;
    [SerializeField, ShowIf("_animateScale")] private float widthScale = 1.03f;
    [SerializeField, ShowIf("_animateScale")] private float heightScale = 1.03f;
    [SerializeField, ShowIf("_animateScale")] private float duration = 0.3f;
    [SerializeField, ShowIf("_animateScale")] private LeanTweenType expandTweenType = LeanTweenType.easeSpring;
    [SerializeField, ShowIf("_animateScale")] private LeanTweenType resetTweenType = LeanTweenType.easeOutSine;
    private RectTransform _transform;
    private Button _button;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        ResetTween();
    }

    public void ExpandTween()
    {
        if (_button.IsInteractable())
        {
            if (_sfxOnSelectHover) _sfxOnSelectHover.Play();
            if (_animateScale)
            {
                _transform.LeanCancel();
                _transform.LeanScale(new Vector3(widthScale,heightScale, 1), duration).setEase(expandTweenType);
            }
            SetSelected(true);
        }
    }

    public void ResetTween()
    {
        if (_animateScale)
        {
            _transform.LeanCancel();
            _transform.LeanScale(new Vector3(1, 1, 1), duration).setEase(resetTweenType);
        }
        SetSelected(false);
    }

    public void OnSelect(BaseEventData eventData) => ExpandTween();
    public void OnDeselect(BaseEventData eventData) => ResetTween();
    public void OnPointerEnter(PointerEventData eventData) => ExpandTween();
    public void OnPointerExit(PointerEventData eventData) => ResetTween();

    public void OnSubmit(BaseEventData eventData)
    {
        if (_sfxOnClick) _sfxOnClick.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_sfxOnClick) _sfxOnClick.Play();
    }

    private void SetSelected(bool selected)
    {
        if (_enableOnSelected) _enableOnSelected.SetActive(selected);
        if (_hideOnSelected) _hideOnSelected.SetActive(!selected);
        if (_tweenToEnableOnSelected)
        {
            if (selected) _tweenToEnableOnSelected.PlayTween();
            else _tweenToEnableOnSelected.StopTween();
        }
    }

    [Button]
    private void SetFirstChildAsEnableObj() => _enableOnSelected = transform.GetChild(0).gameObject;
    [Button]
    private void SetChildAsArrowTween() => _tweenToEnableOnSelected = GetComponentInChildren<MenuArrowTween>();
}
