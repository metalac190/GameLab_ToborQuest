using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraAngleController : MonoBehaviour
{
    public static Action BeforeCameraMove;
    
    [Header("References")]
    [SerializeField] private Transform _camera;
    [SerializeField] private TextMeshProUGUI _text;
    
    [Header("Options")]
    [SerializeField] private int _current;
    [SerializeField] private List<Transform> _options;
    
    private void LateUpdate()
    {
        UpdateCamera();
    }
    
    [Button]
    private void Next()
    {
        _current++;
        if (_current >= _options.Count) _current = 0;
    }
    private void OnNextCamera() => Next();

    [Button]
    private void Previous()
    {
        _current--;
        if (_current < 0) _current = _options.Count - 1;
    }
    private void OnPreviousCamera() => Previous();

    [Button]
    private void UpdateCamera()
    {
        BeforeCameraMove?.Invoke();
        
        _camera.position = _options[_current].position;
        _camera.rotation = _options[_current].rotation;

        _text.text = _options[_current].name;
    }
}
