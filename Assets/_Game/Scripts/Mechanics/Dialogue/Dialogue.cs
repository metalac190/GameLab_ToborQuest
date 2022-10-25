﻿using System.Collections;
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

	
	
	public string Speaker => _speaker;
	public string Text => _text[Random.Range(0, _text.Count - 1)];
	// public string Text => _text[0];
	public SFXEvent DialogueSFX => _dialogueSoundEffect;
	public Sprite SpriteClosedMouth => _spriteClosedMouth;
	public Sprite SpriteOpenMouth => _spriteOpenMouth;

	
	public void RunDialogue()
	{
		if (DialogueSystem.Instance) DialogueSystem.Instance.RunDialogue(this);
		else Debug.LogWarning("No Dialogue System in Place");
	}
}