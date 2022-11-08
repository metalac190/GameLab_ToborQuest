using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Randomizer", menuName = "DialogueSystem/DialogueRandomizer")]
public class DialogueRandomizer : ScriptableObject
{
	[SerializeField] List<Dialogue> _listOfDialogues = new List<Dialogue>();
	
	public void RunDialogue()
	{
		_listOfDialogues[Random.Range(0, _listOfDialogues.Count)].RunDialogue();
	}
}
