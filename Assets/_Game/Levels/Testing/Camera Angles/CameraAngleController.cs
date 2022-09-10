using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngleController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private int _current;
    [SerializeField] private List<Transform> _options;

    [Button]
    private void Next()
    {
        _current++;
        if (_current >= _options.Count) _current = 0;
    }

    [Button]
    private void Previous()
    {
        _current--;
        if (_current < 0) _current = _options.Count - 1;
    }

    private void Update()
    {
        _camera.position = _options[_current].position;
        _camera.rotation = _options[_current].rotation;
    }
}
