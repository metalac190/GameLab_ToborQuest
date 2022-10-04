using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
	public static DialogueSystem Instance;
	
	[SerializeField] private TextMeshProUGUI _speaker;
	[SerializeField] private TextMeshProUGUI _text;

	private void Awake()
	{
		Instance = this;
	}

	public void RunDialogue(Dialogue dialogue)
	{
		_speaker.text = dialogue.Speaker;
		_text.text = dialogue.Text;
	}
}
