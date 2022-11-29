 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSettings : MonoBehaviour
{
    [SerializeField, Range(0, 2)] private int _activeQuality;
    [SerializeField] private GameObject _lowActive;
    [SerializeField] private GameObject _medActive;
	[SerializeField] private GameObject _highActive;
    
	[SerializeField, Range(0, 2)] private int _activeWindowMode;
	[SerializeField] private GameObject _fullscreen;
	[SerializeField] private GameObject _borderless;
	[SerializeField] private GameObject _windowed;

    private void Start()
    {
        // TODO: Set Initial Quality Value
	    UpdateActiveQuality();
	    UpdateWindowMode();
    }

    public void SetLowActive() => SetQuality(0);
    public void SetMedActive() => SetQuality(1);
	public void SetHighActive() => SetQuality(2);

    private void SetQuality(int quality)
    {
        _activeQuality = quality;
        UpdateActiveQuality();
    }

	private void UpdateActiveQuality()
    {
	    QualitySettings.SetQualityLevel(_activeQuality);
	    
        _lowActive.SetActive(_activeQuality == 0);
        _medActive.SetActive(_activeQuality == 1);
        _highActive.SetActive(_activeQuality == 2);
    }
    
	public void SetFullscreen() => SetWindowMode(0);
	public void SetBorderless() => SetWindowMode(1);
	public void SetWindowed() => SetWindowMode(2);
	
	private void SetWindowMode(int mode)
	{
		_activeWindowMode = mode;
		UpdateWindowMode();
	}
	
	private void UpdateWindowMode()
	{
		Screen.fullScreenMode = _activeWindowMode switch
		{
			0 => FullScreenMode.ExclusiveFullScreen,
			1 => FullScreenMode.FullScreenWindow,
			2 => FullScreenMode.Windowed,
			_ => FullScreenMode.FullScreenWindow
		};
		_fullscreen.SetActive(_activeWindowMode == 0);
		_borderless.SetActive(_activeWindowMode == 1);
		_windowed.SetActive(_activeWindowMode == 2);
	}
}
