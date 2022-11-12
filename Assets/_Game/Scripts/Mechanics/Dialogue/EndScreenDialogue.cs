using UnityEngine;

[CreateAssetMenu(menuName = "DialogueSystem/EndScreenDialogue", fileName = "New End Screen Dialogue")]
public class EndScreenDialogue : ScriptableObject
{
    [SerializeField] string _dialogueText;
    [SerializeField] Sprite _speakerSprite;

    public string DialogueText => _dialogueText;
    public Sprite SpeakerSprite => _speakerSprite;

    public void RunDialogue()
    {
        if (!EndScreenDialogueSystem.Instance) Debug.LogWarning("No End Screen Dialogue System in place!");
        else EndScreenDialogueSystem.Instance.RunDialogue(this); 
    }
}