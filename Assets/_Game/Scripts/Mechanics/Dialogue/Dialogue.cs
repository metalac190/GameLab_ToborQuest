using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoundSystem;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject
{
	[Header("Data")]
	[SerializeField] private string _speaker;
	[SerializeField] private float _typingSpeed = 0.04f;
	[SerializeField] private List<string> _text;

	[Header("Animation Settings")]
	[SerializeField] private Sprite _spriteClosedMouth;
	[SerializeField] private Sprite _spriteOpenMouth;
	[SerializeField] private float _dialogueScreenEnterTime = 0.2f;
	[SerializeField] private float _animationSpeed = 0.2f;
	[SerializeField] private float _dialogueScreenExitTime = 0.2f;
	[SerializeField] private float _dialogueDuration = 2f;
	[SerializeField, ReadOnly] private bool _isSequence = false;

	[Header("Sound Settings")]
	// [SerializeField] private AudioClip _dialogueSoundEffect;
	[SerializeField] private SFXEvent _dialogueSoundEffect;
	[SerializeField, ReadOnly] private float _audioClipDuration;
	[SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

	[Header("Tobor Settings")]
	[SerializeField] private bool _freezeTobor;



	public string Speaker => _speaker;
	public string Text => _text[Random.Range(0, _text.Count - 1)];
	public float TypingSpeed => _typingSpeed;
	public AudioClip DialogueSFX => _dialogueSoundEffect != null ? _dialogueSoundEffect.SFXSound : null;
	public float DialogueVolume => _volume;
	public Sprite SpriteClosedMouth => _spriteClosedMouth;
	public Sprite SpriteOpenMouth => _spriteOpenMouth;
	public bool FreezeTobor => _freezeTobor;
	public float TimeToEnter => _dialogueScreenEnterTime;
	public float TimeToExit => _dialogueScreenExitTime;
	public float DialogueDuration => Mathf.Max(_dialogueDuration, _audioClipDuration) + _dialogueScreenEnterTime + _dialogueScreenExitTime + 1f;
	public float AnimationSpeed => _animationSpeed;
	public bool IsSequence {
		get { return _isSequence; } 
		set { _isSequence = value; }
	}

	private void OnValidate()
	{
		_audioClipDuration = _dialogueSoundEffect != null ? _dialogueSoundEffect.SFXSound.length : 0;
	}

	[Button(Spacing = 20, Mode = ButtonMode.InPlayMode)]
	public void RunDialogue()
	{
		if (ExtrasSettings.DialogueDisabled) { return; }

		if (DialogueSystem.Instance)
		{
			if (DialogueSystem.Instance.CurrentDialogue == null) 
			{ 
				DialogueSystem.Instance.RunDialogue(this);
				return; 
			}
			if (_isSequence) { DialogueSystem.Instance.RunDialogue(this); }
			else if (!DialogueSystem.Instance.CurrentDialogue.IsSequence && !_isSequence) { DialogueSystem.Instance.RunDialogue(this); }
			else if (DialogueSystem.Instance.CurrentDialogue.IsSequence && !_isSequence) { return; }
		}
		else Debug.LogWarning("No Dialogue System in Place");
	}
}