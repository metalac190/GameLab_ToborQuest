using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRoll : MonoBehaviour
{
	[SerializeField] private CanvasGroup _group;
	[SerializeField] private RectTransform _creditsTransform;
	[SerializeField] private float _timeBeforeScrolling = 2;
	[SerializeField] private float _scrollTime = 10;
	[SerializeField] private float _extraHeight = 0;
	[SerializeField] private GameObject _selectAfter;
	
	public static bool CreditsActive;
	
	private bool _creditsRolling;
	
	[Button(Mode = ButtonMode.InPlayMode)]
	public void RollCredits()
	{
		if (_creditsRolling) return;
		_creditsRolling = true;
		StartCoroutine(RollCreditsRoutine());
	}
	
	private IEnumerator RollCreditsRoutine()
	{
		CreditsActive = true;
		Cursor.visible = false;
		_group.interactable = false;
		CGSC.MouseKeyboardManager.UpdateSelected(null);
		for (float t = 0; t < _timeBeforeScrolling; t += Time.deltaTime)
		{
			yield return null;
		}
		var rect = _creditsTransform.anchoredPosition;
		float yMax = _creditsTransform.rect.height + _extraHeight;
		float invertedScrollTime = 1f / _scrollTime;
		for (float t = 0; t < 1; t += Time.deltaTime * invertedScrollTime)
		{
			rect.y = t * yMax;
			_creditsTransform.anchoredPosition = rect;
			yield return null;
		}
		rect.y = yMax;
		_creditsTransform.anchoredPosition = rect;
		_group.interactable = true;
		CreditsActive = false;
		Cursor.visible = true;
		CGSC.MouseKeyboardManager.UpdateSelected(_selectAfter);
	}
}
