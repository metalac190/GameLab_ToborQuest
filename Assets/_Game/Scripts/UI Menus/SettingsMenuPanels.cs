using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsMenuPanels : MonoBehaviour
{
	[SerializeField] private CanvasGroup _audioGroup;
	[SerializeField] private GameObject _audioSelected;
	[SerializeField] private GameObject _audioDefaultSelected;
	[SerializeField] private CanvasGroup _visualGroup;
	[SerializeField] private GameObject _visualSelected;
	[SerializeField] private CanvasGroup _controlsGroup;
	[SerializeField] private GameObject _controlsSelected;
	[SerializeField] private CanvasGroup _extraGroup;
	[SerializeField] private GameObject _extraSelected;

	public void SaveSettings() => CGSC.SettingsSaver.ButtonSave();
	
	[Button]
	public void SetAudioActive() => SetGroupActive(0);
	[Button]
	public void SetVisualActive() => SetGroupActive(1);
	[Button]
	public void SetControlsActive() => SetGroupActive(2);
	[Button]
	public void SetExtraActive() => SetGroupActive(3);
	
	public void SetDefaultActive()
	{
		SetAudioActive();
		SetSelected(_audioDefaultSelected);
	}
	
	private void SetGroupActive(int group)
	{
		SetGroupActive(_audioGroup, group == 0);
		SetGroupActive(_visualGroup, group == 1);
		SetGroupActive(_controlsGroup, group == 2);
		SetGroupActive(_extraGroup, group == 3);

		GameObject toSelect = group switch
		{
			0 => _audioSelected,
			1 => _visualSelected,
			2 => _controlsSelected,
			3 => _extraSelected,
			_ => null
		};
		if (toSelect) SetSelected(toSelect);
	}
	
	private static void SetSelected(GameObject obj)
	{
		CGSC.MouseKeyboardManager.UpdateSelected(obj);
	}
	
	private static void SetGroupActive(CanvasGroup group, bool active = true)
	{
		if (!group) return;
		group.alpha = active ? 1 : 0;
		group.interactable = active;
		group.blocksRaycasts = active;
	}
}
