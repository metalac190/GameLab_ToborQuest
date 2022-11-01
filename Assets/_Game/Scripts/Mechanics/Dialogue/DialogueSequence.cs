using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [SerializeField] List<Dialogue> _dialogues = new List<Dialogue>();
    [SerializeField, ReadOnly] bool _skipDialogue = false;

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
        _skipDialogue = false;
        foreach (Dialogue d in _dialogues)
        {
            d.RunDialogue();
            for (float t = 0; t < d.DialogueDuration; t += Time.deltaTime) {
                if (_skipDialogue)
                {
                    Debug.Log("skipped");
                    _skipDialogue = false;
                    break;
                }
                yield return null;
            }
        }
    }

    private void Start()
    {
        DialogueSystem.OnSkipDialogue += () => _skipDialogue = true;
    }
}
