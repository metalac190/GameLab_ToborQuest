using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
	[SerializeField] private string _speaker;
	[SerializeField, TextArea] private string _text;
	
	public string Speaker => _speaker;
	public string Text => _text;
	
	[Button]
	public void RunDialogue()
	{
		if (DialogueSystem.Instance) DialogueSystem.Instance.RunDialogue(this);
		else Debug.LogWarning("No Dialogue Sytstem in Place");
	}
}
