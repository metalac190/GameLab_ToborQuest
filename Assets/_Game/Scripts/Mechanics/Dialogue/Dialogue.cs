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
	[SerializeField] float _typingSpeed = 0.04f;
	[SerializeField, TextArea] private List<string> _text;

	[Header("Animation Settings")]
	[SerializeField] private Sprite _spriteClosedMouth;
	[SerializeField] private Sprite _spriteOpenMouth;
	[SerializeField] private float _dialogueScreenEnterTime = 0.2f;
	[SerializeField] private float _animationSpeed = 0.2f;
	[SerializeField] private float _dialogueScreenExitTime = 0.2f;
	[SerializeField] private float _dialogueDuration = 2f;

	[Header("Sound Settings")]
	[SerializeField] private AudioClip _dialogueSoundEffect;
	[SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

	[Header("Tobor Settings")]
	[SerializeField] private bool _freezeTobor;



	public string Speaker => _speaker;
	public string Text => _text[Random.Range(0, _text.Count - 1)];
	public float TypingSpeed => _typingSpeed;
	public AudioClip DialogueSFX => _dialogueSoundEffect;
	public float DialogueVolume => _volume;
	public Sprite SpriteClosedMouth => _spriteClosedMouth;
	public Sprite SpriteOpenMouth => _spriteOpenMouth;
	public bool FreezeTobor => _freezeTobor;
	public float TimeToEnter => _dialogueScreenEnterTime;
	public float TimeToExit => _dialogueScreenExitTime;
	public float DialogueDuration => _dialogueDuration + _dialogueScreenEnterTime + _dialogueScreenExitTime;
	public float AnimationSpeed => _animationSpeed;

	
	public void RunDialogue()
	{
		if (DialogueSystem.Instance)
		{
			DialogueSystem.Instance.RunDialogue(this);
		}
		else Debug.LogWarning("No Dialogue System in Place");
	}
}