using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
	public static DialogueSystem Instance;
	
	[SerializeField, HighlightIfNull] GameObject _panel;
	[SerializeField] DialogueAnimator _animator;
	[SerializeField] float _typingSpeed = 0.04f;
	[SerializeField] private TextMeshProUGUI _speaker;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _speakerSprite;
	

	private Sprite _speakerSpriteClosed;
	private Sprite _speakerSpriteOpen;
	


	private Dialogue _dialogue;
	
	private int counterMax;

	private Coroutine _displayLineCoroutine;
	private bool _talking;

	int counter = 0;

	private void Awake()
	{
		Instance = this;
		counter = 0;
		counterMax = 0;
		_talking = false;

	}

	void OnValidate()
	{
		if (_panel == null) { _panel = gameObject; }
		if (_animator == null) { _animator = GetComponent<DialogueAnimator>(); }
	}

	void Start()
	{
		_panel = gameObject;
	}

	void Update()
	{
		if (_talking && counter < counterMax) { counter++; }
		else if (_talking && counter >= counterMax)
		{
			_talking = false;
			counter = 0;
			counterMax = 0;
			
		}
	}

	public void RunDialogue(Dialogue dialogue)
	{
		
		_dialogue = dialogue;
		
		counterMax = (int)dialogue.DialogueDuration * 60;
		
		if (_displayLineCoroutine != null) { StopCoroutine(_displayLineCoroutine); }

		if (!dialogue.Speaker.Equals("")) { _displayLineCoroutine = StartCoroutine(PrintName(dialogue.Speaker)); }

		if (!dialogue.Text.Equals("")) { _displayLineCoroutine = StartCoroutine(PrintText(dialogue.Text)); }

		_speakerSpriteOpen = dialogue.SpriteOpenMouth;
		_speakerSpriteClosed = dialogue.SpriteClosedMouth;

		_animator.IntroAndExitAnimation(dialogue.TimeToEnter, dialogue.DialogueDuration, dialogue.TimeToExit);


		if (dialogue.DialogueSFX) { dialogue.DialogueSFX.Play(); }
		
		if (_speakerSpriteClosed != null) 
		{ 
			StartCoroutine(AnimateSprite(dialogue.AnimationSpeed)); 
			_talking = true;
		}
	}
	


	IEnumerator AnimateSprite(float _timeBetween)
	{
		
		while(counter < counterMax)
		{
			_speakerSprite.sprite = _speakerSpriteOpen;
			yield return new WaitForSeconds(_timeBetween);
			_speakerSprite.sprite = _speakerSpriteClosed;
			yield return new WaitForSeconds(_timeBetween);
		}
	}

	IEnumerator PrintText(string _dialogueText)
	{
		_text.text = "";

		foreach (char c in _dialogueText)
		{
			_text.text += c;
			yield return new WaitForSeconds(_typingSpeed);
		}

	}

	IEnumerator PrintName(string _dialogueName)
	{
		_speaker.text = "";

		foreach (char c in _dialogueName)
		{
			_speaker.text += c;
			yield return new WaitForSeconds(_typingSpeed);
		}

	}
	
	

	
}
