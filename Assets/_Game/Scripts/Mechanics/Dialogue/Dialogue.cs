using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoundSystem;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject
{
	[SerializeField] private string _speaker;
	[SerializeField, TextArea] private List<string> _text;
	[SerializeField] private Sprite _spriteClosedMouth;
	[SerializeField] private Sprite _spriteOpenMouth;
	[SerializeField] private SFXEvent _dialogueSoundEffect;
	[SerializeField] private float _dialogueScreenEnterTime = 0.2f;
	[SerializeField] private float _animationSpeed = 0.2f;
	[SerializeField] private float _dialogueScreenExitTime = 0.2f;
	[SerializeField] private float _dialogueDuration = 2f;
	[SerializeField] private bool _freezeTobor;
	

	
	
	public string Speaker => _speaker;
	public string Text => _text[Random.Range(0, _text.Count - 1)];
	// public string Text => _text[0];
	public SFXEvent DialogueSFX => _dialogueSoundEffect;
	public Sprite SpriteClosedMouth => _spriteClosedMouth;
	public Sprite SpriteOpenMouth => _spriteOpenMouth;
	public bool FreezeTobor => _freezeTobor;
	public float TimeToEnter => _dialogueScreenEnterTime;
	public float TimeToExit => _dialogueScreenExitTime;
	public float DialogueDuration => _dialogueDuration;
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