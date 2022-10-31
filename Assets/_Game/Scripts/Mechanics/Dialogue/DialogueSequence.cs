using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [SerializeField] List<Dialogue> _dialogues = new List<Dialogue>();


    public void RunDialogues()
    {
        if (!DialogueSystem.Instance)
        {
            Debug.LogWarning("No Dialogue System in Place");
        }
        StartCoroutine(RunDialogueAfterTime());
    }

    IEnumerator RunDialogueAfterTime()
    {
        foreach (Dialogue d in _dialogues)
        {
            d.RunDialogue();
            yield return new WaitForSeconds(d.DialogueDuration);
        }
    }
}
