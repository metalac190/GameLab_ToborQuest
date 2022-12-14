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
	public static Action OnResetDialogue = delegate { };

	[SerializeField, ReadOnly] private Dialogue _currentDialogue;
	[SerializeField, ReadOnly] private Sprite _speakerSpriteClosed;
	[SerializeField, ReadOnly] private Sprite _speakerSpriteOpen;
	[SerializeField, ReadOnly] private bool _printingText;
	[SerializeField, ReadOnly] private int skip = 0;
	[SerializeField, ReadOnly] private int counterMax;
	[SerializeField, ReadOnly] private float _dialogueTime;
	[SerializeField, ReadOnly] private bool _talking;
	[SerializeField, ReadOnly] private bool _paused;
	[SerializeField, ReadOnly] private int counter;
	
	private AudioSource _source;
	private MovementController _movement;

	private Coroutine _displayLineCoroutine;
	private Coroutine _panelAnimationCoroutine;
	private Coroutine _freezeToborCoroutine;
	private Coroutine _animateSpriteCoroutine;

	public Dialogue CurrentDialogue => _currentDialogue;


	private void Awake()
	{
		Instance = this;
		_animator = GetComponent<DialogueAnimator>();
		_source = GetComponent<AudioSource>();
		_movement = FindObjectOfType<MovementController>();

	}

	private void OnEnable()
	{
		CGSC.OnPause += OnPauseGame;
	}

	private void OnDisable()
	{
		CGSC.OnUnpause -= OnPauseGame;
	}

	private void Start()
	{
		if (ExtrasSettings.DialogueDisabled)
		{
			gameObject.SetActive(false);
			return;
		}
		
		counter = 0;
		counterMax = 0;
		_talking = false;
		_paused = false;
	}


	private void Update()
	{
		if (_talking && counter < counterMax)
		{
			counter++;
		}
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
		if (!_talking) return;
		if (_currentDialogue.FreezeTobor || !_paused)
		{
			skip++;
			if (skip == 1) { _onFirstSkipEvent?.Invoke(); }
			if (skip == 2) { _onSecondSkipEvent?.Invoke(); }
		}
	}


	private void OnPauseGame()
	{
		_paused = !_paused;
	}

	public void RunDialogue(Dialogue dialogue)
	{
		if (!gameObject.activeSelf) return;
		HandleDialogueReset(dialogue);
		HandleDialogueIn(dialogue);
		HandleDialogueRun(dialogue);
	}

	#region HandleMethods

	private void HandleDialogueIn(Dialogue _currDialogue)
	{
		_currentDialogue = _currDialogue;
		_animator.IntroAnimation(_currDialogue.TimeToEnter);
		CheckDialogueTime(_currDialogue);
		counterMax = (int)_currDialogue.DialogueDuration * 60;
		_speakerSpriteOpen = _currDialogue.SpriteOpenMouth;
		_speakerSpriteClosed = _currDialogue.SpriteClosedMouth;

		if (_currDialogue.FreezeTobor) 
		{ 
			_movement.SetActive(false);
			_freezeToborCoroutine = StartCoroutine(HandleToborFreeze(_dialogueTime)); 
		}
		_panelAnimationCoroutine = StartCoroutine(HandlePanelAnimation(_dialogueTime, _currDialogue.TimeToExit));

	}

	private void HandleDialogueRun(Dialogue _currDialogue)
	{
		
		if (!_currDialogue.Text.Equals("")) { _displayLineCoroutine = StartCoroutine(PrintText(_currDialogue.Text, _currDialogue.TypingSpeed)); }
		if (!_currDialogue.Speaker.Equals("")) { _speaker.text = _currDialogue.Speaker; }

        if (_currDialogue.DialogueSFX != null) { _source.PlayOneShot(_currDialogue.DialogueSFX, _currDialogue.DialogueVolume); }
		if (_speakerSpriteClosed != null) 
		{
			_animateSpriteCoroutine = StartCoroutine(AnimateSprite(_currDialogue.AnimationSpeed)); 
			_talking = true;
			skip = 0;
		}

	}

	private void HandleDialogueReset(Dialogue _currDialogue)
	{
		
		_animator.CancelAnimations();
		_source.Stop();
		if (_displayLineCoroutine != null) { StopCoroutine(_displayLineCoroutine); }
		if (_panelAnimationCoroutine != null) StopCoroutine(_panelAnimationCoroutine);
		if (_freezeToborCoroutine != null) StopCoroutine(_freezeToborCoroutine);
		if (_animateSpriteCoroutine != null) StopCoroutine(_animateSpriteCoroutine);
		counter = 0;
		counterMax = 0;
		_talking = false;
		skip = 0;
        if (_currentDialogue != null && _currentDialogue.FreezeTobor) _movement.SetActive(true);
        _currentDialogue = null;

    }

	#endregion


	private void CheckDialogueTime(Dialogue dialogue)
	{
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
	}
	
	#region Coroutines

	private IEnumerator HandleToborFreeze(float s)
	{
		yield return new WaitForSeconds(s);
		_movement.SetActive(true);
	}

	private IEnumerator HandlePanelAnimation( float wait, float exit)
	{
		yield return new WaitForSeconds(wait);
		_animator.ExitAnimation(exit);
	}

	private IEnumerator AnimateSprite(float _timeBetween)
	{
		
		while(_printingText)
		{
			_speakerSprite.sprite = _speakerSpriteOpen;
			yield return new WaitForSeconds(_timeBetween);
			_speakerSprite.sprite = _speakerSpriteClosed;
			yield return new WaitForSeconds(_timeBetween);
		}
	}

	private IEnumerator PrintText(string _dialogueText, float _typingSpeed)
	{
		_text.text = "";


		for (int i = 0; i < _dialogueText.Length + 1; i++)
		{
			if (i < _dialogueText.Length)
			{
				_printingText = true;
				_text.text += _dialogueText[i];
				yield return new WaitForSeconds(_typingSpeed);
			}
			else
            {
				_printingText = false;
				_source.Stop();
            }
		}

	}

	#endregion

	
}
