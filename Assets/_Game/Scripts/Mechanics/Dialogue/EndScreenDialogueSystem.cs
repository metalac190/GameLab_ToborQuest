using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EndScreenDialogueSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Sprite _speakingSprite;
    [SerializeField] string _dialogueText;

    [Header("References")]
    [SerializeField] TextMeshProUGUI _dialogueTMP;
    [SerializeField] Image _spriteToBeChanged;



    void Start()
    {
        if (!_dialogueText.Equals("")) _dialogueTMP.text = _dialogueText;
        if (_speakingSprite) _spriteToBeChanged.sprite = _speakingSprite;   
    }
}
