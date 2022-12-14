using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [SerializeField] private bool _onlyRunInQuest;
	[SerializeField] private bool _onlyRunOnce;
    [SerializeField] List<Dialogue> _dialogues = new List<Dialogue>();
    [SerializeField, ReadOnly] bool _skipDialogue = false;

	private bool _hasRunBefore;

    Coroutine _runDialogueCoroutine;

    public void RunDialogues()
	{
		if (_onlyRunOnce && _hasRunBefore) return;
		_hasRunBefore = true;
        if (_onlyRunInQuest && !CGSC.PlayingQuest) return;
        if (_runDialogueCoroutine != null) { StopCoroutine(_runDialogueCoroutine); }
        if (!DialogueSystem.Instance)
        {
            Debug.LogWarning("No Dialogue System in Place");
        }
        _runDialogueCoroutine = StartCoroutine(RunDialogueAfterTime());
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
                    //Debug.Log("skipped");
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
       
        
        foreach (Dialogue d in _dialogues)
        {
            d.IsSequence = true;
        }
    }

    // void OnEnable()
    // {
    //     DialogueSystem.OnResetDialogue += ResetDialogue;
    // }

    // void OnDisable()
    // {
    //     DialogueSystem.OnResetDialogue -= ResetDialogue;
    // }

    // void ResetDialogue()
    // {
    //     if (_runDialogueCoroutine != null) { StopCoroutine(_runDialogueCoroutine); }
    //     Debug.Log("Stopped Old Coroutine...now Moving On To new One!");
    // }
}
