using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EndScreenDialogueSystem : MonoBehaviour
{
    public static EndScreenDialogueSystem Instance {get; private set;}
    
    [Header("References")]
    [SerializeField] TextMeshProUGUI _dialogueTMP;
    [SerializeField] Image _spriteToBeChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void RunDialogue(EndScreenDialogue dialogue)
    {
        _spriteToBeChanged.sprite = dialogue.SpeakerSprite;
        _dialogueTMP.text = dialogue.DialogueText;
    }

}
