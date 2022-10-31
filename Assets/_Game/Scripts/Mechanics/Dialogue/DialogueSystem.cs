using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
	public static DialogueSystem Instance;
	
	[SerializeField] public DialogueAnimator _animator;
	[SerializeField] float _typingSpeed = 0.04f;
	[SerializeField] private TextMeshProUGUI _speaker;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _speakerSprite;
	

	private Sprite _speakerSpriteClosed;
	private Sprite _speakerSpriteOpen;
	private bool _printingText = false;
	private AudioSource _source;


	
	private int counterMax;

	private Coroutine _displayLineCoroutine;
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
		
		counter = 0;
		counterMax = 0;
		_talking = false;
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
		_animator.IntroAnimation(dialogue.TimeToEnter);



		StartCoroutine(HandlePanelAnimation(dialogue.DialogueDuration, dialogue.TimeToExit));


		counterMax = (int)dialogue.DialogueDuration * 60;
		counterMax += (int)dialogue.TimeToEnter * 60;
		counterMax += (int)dialogue.TimeToExit * 60;


		if (_displayLineCoroutine != null) { StopCoroutine(_displayLineCoroutine); }

		if (!dialogue.Speaker.Equals("")) { _speaker.text = dialogue.Speaker; }

		if (!dialogue.Text.Equals("")) { _displayLineCoroutine = StartCoroutine(PrintText(dialogue.Text)); }

		_speakerSpriteOpen = dialogue.SpriteOpenMouth;
		_speakerSpriteClosed = dialogue.SpriteClosedMouth;


        if (dialogue.DialogueSFX) { _source.PlayOneShot(dialogue.DialogueSFX); }
		
		if (_speakerSpriteClosed != null) 
		{ 
			StartCoroutine(AnimateSprite(dialogue.AnimationSpeed)); 
			_talking = true;
		}
	}


    #region Coroutines
    IEnumerator HandlePanelAnimation( float wait, float exit)
	{
		yield return new WaitForSeconds(wait);
		_animator.ExitAnimation(exit);
	}
	


	IEnumerator AnimateSprite(float _timeBetween)
	{
		
		while(_printingText)
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

	IEnumerator PrintName(string _dialogueName)
	{
		_speaker.text = "";

		foreach (char c in _dialogueName)
		{
			_speaker.text += c;
			yield return new WaitForSeconds(0);
		}

	}
	#endregion

	#region SFX Events

	

	#endregion

}
