using System.Collections.Generic;
using UnityEngine;

public class UtilityDemo : ScriptableObject
{
    [Header("Read Only")]
    [SerializeField] private float _regularValue;
    [SerializeField, ReadOnly] private float _readOnlyValue;

    [SerializeField] private List<GameObject> _regularList;
    [SerializeField, ReadOnly] private List<GameObject> _readOnlyList;

    [Header("Back Highlights")]
    [SerializeField, Highlight(0.1f, 1f, 0.1f)] private bool _greenField;
    [SerializeField, Highlight(ColorField.Blue)] private int _blueField;
    [SerializeField, Highlight(250, 200, 15)] private float _yellowField;
    [SerializeField, Highlight(System.Drawing.KnownColor.Red)] private GameObject _redField;

    [Header("Text Highlights")]
    [SerializeField, Highlight(0.1f, 1f, 0.1f, HighlightMode.Text)] private bool _greenField2;
    [SerializeField, Highlight(ColorField.Blue, HighlightMode.Text)] private int _blueField2;
    [SerializeField, Highlight(250, 200, 15, HighlightMode.Text)] private bool _yellowField2;
    [SerializeField, Highlight(System.Drawing.KnownColor.Red, HighlightMode.Text)] private GameObject _redField2;

    [Header("Highlight If")]
    [SerializeField] private bool _test;
    [SerializeField, HighlightIf("_test")] private float _testHighlight;

    [Header("Highlight If Null")]
    [SerializeField, HighlightIfNull] private GameObject _defaultObj;
    [SerializeField, HighlightIfNull(ColorField.Green)] private GameObject _greenObj;
    [SerializeField, HighlightIfNull] private bool _invalidField;

    [Header("Show If")]
    [SerializeField] private bool _show;
    [SerializeField, ShowIf("_show")] private float _value;

    [SerializeField, ShowIf("_show")] private bool _secondardShow;
    [SerializeField, ShowIf("_show", "_secondardShow")] private float _secondaryValue;

    [Button(Spacing = 20)]
    private void NormalButton()
    {
    }

    [Button(Mode = ButtonMode.InPlayMode)]
    private void PlayModeOnlyButton()
    {
    }

    [Button(Mode = ButtonMode.NotInPlayMode)]
    private void NotInPlayModeButton()
    {
    }

    [Button(Spacing = 10, Color = ColorField.Green)]
    private void GreenButton()
    {
    }

    [Button(Color = ColorField.Red)]
    private void RedButton()
    {
    }
}
