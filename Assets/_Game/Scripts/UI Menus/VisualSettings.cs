 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSettings : MonoBehaviour
{
    [SerializeField, Range(0, 2)] private int _activeQuality;
    [SerializeField] private GameObject _lowActive;
    [SerializeField] private GameObject _medActive;
    [SerializeField] private GameObject _highActive;

    private void Start()
    {
        // TODO: Set Initial Quality Value
        UpdateActive();
    }

    public void SetLowActive() => SetQuality(0);
    public void SetMedActive() => SetQuality(1);
    public void SetHighActive() => SetQuality(2);

    private void SetQuality(int quality)
    {
        _activeQuality = quality;
        UpdateActive();
    }

    private void UpdateActive()
    {
        _lowActive.SetActive(_activeQuality == 0);
        _medActive.SetActive(_activeQuality == 1);
        _highActive.SetActive(_activeQuality == 2);
    }
}
