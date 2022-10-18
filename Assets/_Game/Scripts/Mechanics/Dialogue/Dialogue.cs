using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject
{
	[SerializeField] private string _speaker;
	[SerializeField, TextArea] private List<string> _text;
	
	public string Speaker => _speaker;
	public string Text => _text[Random.Range(0, _text.Count - 1)];
	
	[Button]
	public void RunDialogue()
	{
		if (DialogueSystem.Instance) DialogueSystem.Instance.RunDialogue(this);
		else Debug.LogWarning("No Dialogue System in Place");
	}
}