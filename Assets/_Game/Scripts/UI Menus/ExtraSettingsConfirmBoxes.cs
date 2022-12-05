using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraSettingsConfirmBoxes : MonoBehaviour
{
	[SerializeField] private ExtrasSettings _settings;
	
	[SerializeField] private CanvasGroup _raycastBlocker;
	[SerializeField] private CanvasGroup _boxResetSettings;
	[SerializeField] private CanvasGroup _boxResetTimes;
	
	[SerializeField] private GameObject _openResetSettings;
	[SerializeField] private GameObject _closeResetSettings;
	[SerializeField] private GameObject _openResetTimes;
	[SerializeField] private GameObject _closeResetTimes;
	
	public void PromptResetSettings()
	{
		SetGroupActive(_raycastBlocker, true);
		SetGroupActive(_boxResetSettings, true);
		SetGroupActive(_boxResetTimes, false);
		SetSelected(_openResetSettings);
	}
	
	public void PromptResetTimes()
	{
		SetGroupActive(_raycastBlocker, true);
		SetGroupActive(_boxResetSettings, false);
		SetGroupActive(_boxResetTimes, true);
		SetSelected(_openResetTimes);
	}
	
	public void ConfirmResetSettings()
	{
		_settings.ResetAllSettings();
		CloseResetSettings();
	}
	
	public void ConfirmResetTimes()
	{
		_settings.ResetAllTimes();
		CloseResetTimes();
	}
	
	public void CloseResetSettings()
	{
		SetGroupActive(_raycastBlocker, false);
		SetGroupActive(_boxResetSettings, false);
		SetGroupActive(_boxResetTimes, false);
		SetSelected(_closeResetSettings);
	}
	
	public void CloseResetTimes()
	{
		SetGroupActive(_raycastBlocker, false);
		SetGroupActive(_boxResetSettings, false);
		SetGroupActive(_boxResetTimes, false);
		SetSelected(_closeResetTimes);
	}
	
	private static void SetSelected(GameObject obj)
	{
		if (obj) CGSC.MouseKeyboardManager.UpdateSelected(obj);
	}
	
	private void SetGroupActive(CanvasGroup group, bool active)
	{
		group.alpha = active ? 1 : 0;
		group.interactable = active;
		group.blocksRaycasts = active;
	}
}
