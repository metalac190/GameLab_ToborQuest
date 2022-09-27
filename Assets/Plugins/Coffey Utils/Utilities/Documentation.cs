using UnityEngine;

public class Documentation : MonoBehaviour
{
    [SerializeField] private TextAsset _text = null;
    [SerializeField] private Color _textColor = Color.white;

    public string Text => _text != null ? _text.text : "";
}