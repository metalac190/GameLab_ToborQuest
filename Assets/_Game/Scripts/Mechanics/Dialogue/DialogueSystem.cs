using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
	public static DialogueSystem Instance;
	
	[SerializeField] public DialogueAnimator _animator;
	[SerializeField] private TextMeshProUGUI _speaker;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _speakerSprite;
	[SerializeField] private UnityEvent _onFirstSkipEvent;
	[SerializeField] private UnityEvent _onSecondSkipEvent;


	public static Action OnSkipDialogue = delegate { };

	private Sprite _speakerSpriteClosed;
	private Sprite _speakerSpriteOpen;
	private bool _printingText = false;
	private AudioSource _source;
	private MovementController _movement;
	private Dialogue _currentDialogue;


	[SerializeField, ReadOnly] private int skip = 0;
	public int Skip => skip;
	private int counterMax;
	private float _dialogueTime;

	private Coroutine _displayLineCoroutine;
	private Coroutine _panelAnimationCoroutine;
	private Coroutine _freezeToborCoroutine;
	private Coroutine _animateSpriteCoroutine;
	private bool _talking;

	int counter = 0;

	private void Awake()
	{
		Instance = this;
		_animator = GetComponent<DialogueAnimator>();
		_source = GetComponent<AudioSource>();
	}

	void Start()
	{
		if (ExtrasSettings.DialogueDisabled)
		{
			gameObject.SetActive(false);
			return;
		}
		
		counter = 0;
		counterMax = 0;
		_talking = false;
		_movement = GameObject.FindObjectOfType<MovementController>();
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
		
		
		if (skip == 1)
		{
			if (_displayLineCoroutine != null) StopCoroutine(_displayLineCoroutine);
			_text.text = _currentDialogue.Text;
		}

		if (skip == 2)
		{
			if (_panelAnimationCoroutine != null) StopCoroutine(_panelAnimationCoroutine);
			if (_freezeToborCoroutine != null) StopCoroutine(_freezeToborCoroutine);
			if (_animateSpriteCoroutine != null) StopCoroutine(_animateSpriteCoroutine);

			_talking = false;
			counter = 0;
			counterMax = 0;
			_panelAnimationCoroutine = StartCoroutine(HandlePanelAnimation(0.01f, _currentDialogue.TimeToExit));
			_freezeToborCoroutine = StartCoroutine(HandleToborFreeze(0.01f));
			_animator.CancelAnimations();
			OnSkipDialogue?.Invoke();
			_source.Stop();
			skip = 0;
		}
	}

	public static void SkipDialogueStatic(InputAction.CallbackContext context)
	{
		if (Instance) Instance.SkipDialogue();
	}

	[Button]
	public void SkipDialogue()
	{
		if (_currentDialogue.FreezeTobor)
		{
			skip++;
			if (skip == 1) { _onFirstSkipEvent?.Invoke(); }
			if (skip == 2) { _onSecondSkipEvent?.Invoke(); }
		}
	}

	public void RunDialogue(Dialogue dialogue)
	{
		//Debug.Log("test");

		_source.Stop();
		if (_displayLineCoroutine != null) { StopCoroutine(_displayLineCoroutine); }


		_currentDialogue = dialogue;

		if (_panelAnimationCoroutine != null) StopCoroutine(_panelAnimationCoroutine);
		_animator.IntroAnimation(dialogue.TimeToEnter);
		if (dialogue.FreezeTobor) { _movement.SetActive(false); }

		float _timeAmount = 0f;
        foreach (char c in dialogue.Text) { _timeAmount += dialogue.TypingSpeed; }
		if (dialogue.DialogueDuration < _timeAmount) 
		{
			_dialogueTime = _timeAmount;
		}
		else 
		{
			_dialogueTime = dialogue.DialogueDuration;
		}
		
		_panelAnimationCoroutine = StartCoroutine(HandlePanelAnimation(_dialogueTime, dialogue.TimeToExit));
		if (dialogue.FreezeTobor) { _freezeToborCoroutine = StartCoroutine(HandleToborFreeze(_dialogueTime)); }

		counterMax = (int)dialogue.DialogueDuration * 60;


		if (!dialogue.Speaker.Equals("")) { _speaker.text = dialogue.Speaker; }

		if (!dialogue.Text.Equals("")) { _displayLineCoroutine = StartCoroutine(PrintText(dialogue.Text, dialogue.TypingSpeed)); }
		// Debug.Log(dialogue.Text);


		_speakerSpriteOpen = dialogue.SpriteOpenMouth;
		_speakerSpriteClosed = dialogue.SpriteClosedMouth;


        if (dialogue.DialogueSFX) { _source.PlayOneShot(dialogue.DialogueSFX, dialogue.DialogueVolume); }
		
		if (_speakerSpriteClosed != null) 
		{
			_animateSpriteCoroutine = StartCoroutine(AnimateSprite(dialogue.AnimationSpeed)); 
			_talking = true;
		}
	}

	public void FreezeTobor(float _seconds)
	{
		StartCoroutine(HandleToborFreeze(_seconds));
	}
	
	#region Coroutines
	IEnumerator HandleToborFreeze(float s)
	{
		yield return new WaitForSecondsRealtime(s);
		_movement.SetActive(true);
	}

	IEnumerator HandlePanelAnimation( float wait, float exit)
	{
		yield return new WaitForSecondsRealtime(wait);
		_animator.ExitAnimation(exit);
	}
	
	IEnumerator AnimateSprite(float _timeBetween)
	{
		
		while(_printingText)
		{
			_speakerSprite.sprite = _speakerSpriteOpen;
			yield return new WaitForSecondsRealtime(_timeBetween);
			_speakerSprite.sprite = _speakerSpriteClosed;
			yield return new WaitForSecondsRealtime(_timeBetween);
		}
	}

	IEnumerator PrintText(string _dialogueText, float _typingSpeed)
	{
		_text.text = "";


		for (int i = 0; i < _dialogueText.Length + 1; i++)
		{
			if (i < _dialogueText.Length)
			{
				_printingText = true;
				_text.text += _dialogueText[i];
				yield return new WaitForSecondsRealtime(_typingSpeed);
			}
			else
            {
				_printingText = false;
				_source.Stop();
            }
		}

	}

	#endregion

	#region SFX Events

	

	#endregion

}
