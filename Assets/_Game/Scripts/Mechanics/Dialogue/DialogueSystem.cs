using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
	public static DialogueSystem Instance;
	
	[SerializeField] float _typingSpeed = 0.04f;
	[SerializeField] private TextMeshProUGUI _speaker;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _speakerSprite;
	private Sprite _speakerSpriteClosed;
	private Sprite _speakerSpriteOpen;

	private Coroutine _displayLineCoroutine;
	private bool _talking;

	int counter = 0;

	private void Awake()
	{
		Instance = this;
		counter = 0;
		_talking = false;

	}

	void Update()
	{
		if (_talking && counter < 300) { counter++; }
		else if (_talking && counter >= 300)
		{
			_talking = false;
			counter = 0;
		}
	}

	public void RunDialogue(Dialogue dialogue)
	{
		if (_displayLineCoroutine != null) { StopCoroutine(_displayLineCoroutine); }

		_speaker.text = dialogue.Speaker;

		if (!dialogue.Text.Equals("")) { _displayLineCoroutine = StartCoroutine(Print(dialogue.Text)); }

		_speakerSpriteOpen = dialogue.SpriteOpenMouth;
		_speakerSpriteClosed = dialogue.SpriteClosedMouth;

		dialogue.DialogueSFX.Play();
		
		if (_speakerSpriteClosed != null) 
		{ 
			StartCoroutine(AnimateSprite(1f)); 
			_talking = true;
		}
	}

	IEnumerator AnimateSprite(float _timeBetween)
	{
		
		while(counter < 300)
		{
			_speakerSprite.sprite = _speakerSpriteOpen;
			yield return new WaitForSeconds(_timeBetween);
			_speakerSprite.sprite = _speakerSpriteClosed;
			yield return new WaitForSeconds(_timeBetween);
		}
	}

	IEnumerator Print(string _dialogueText)
	{
		_text.text = "";

		foreach (char c in _dialogueText)
		{
			_text.text += c;
			yield return new WaitForSeconds(_typingSpeed);
		}

	}

	
}
