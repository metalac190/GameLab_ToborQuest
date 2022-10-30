using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [SerializeField] List<Dialogue> _dialogues = new List<Dialogue>();

    float _totalTime;

    public void RunDialogues()
    {
        foreach (Dialogue _d in _dialogues)
        {
            _totalTime += _d.DialogueDuration;
        }

        if (DialogueSystem.Instance)
        {
            //DialogueSystem.Instance._animator.IntroAnimation(1f);
            //DialogueSystem.Instance._totalWaitTime = _totalTime;

            ////DialogueSystem.Instance._animator.IntroAndExitAnimation(1f, _totalTime, 1f);
            //StartCoroutine(HandleDisabledAnimation(_totalTime));
            Debug.Log($"[Dialogue System]: {_totalTime}");
        }
        else
        {
            Debug.LogWarning("No Dialogue System in Place");
        }

        StartCoroutine(RunDialogueAfterTime());

    }

    //IEnumerator HandleDisabledAnimation(float _time)
    //{
    //    yield return new WaitForSeconds(_time);

    //    Debug.Log("[Dialogue System]: Animations Enabled");
    //}

    IEnumerator RunDialogueAfterTime()
    {
      


        foreach (Dialogue d in _dialogues)
        {

            d.RunDialogue();
            yield return new WaitForSeconds(d.DialogueDuration);

        }
        
    }
}
