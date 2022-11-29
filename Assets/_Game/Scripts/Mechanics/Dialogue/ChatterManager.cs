using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatterManager : MonoBehaviour
{
    [SerializeField] string[] _chatterDialogues;
    [SerializeField] Transform[] _npcTransforms;
    [SerializeField] GameObject _textObject;
    [SerializeField] float _timeBetweenChatter = 4f; 
    [SerializeField] float _heightAboveNPC = 3f;
    [SerializeField] bool _enableChatter = true;
    public bool EnableChatter
    {
        get { return _enableChatter; }
        set { _enableChatter = value; }
    }

    bool _followObject;
    bool _runOnce = false;
    Transform _currentTarget;
    string _currentDialogue;
    TextMeshPro _text;

    Coroutine _chatterCoroutine;
    Coroutine _printStringCoroutine;

    void Start()
    {
        _textObject.SetActive(false);
        _text = _textObject.GetComponent<TextMeshPro>();
        _chatterCoroutine = StartCoroutine(ChatterEnabler());
    }

    void Update()
    {
        if (_followObject && _enableChatter)
        {

            transform.position = new Vector3(_currentTarget.position.x, _currentTarget.position.y + _heightAboveNPC, _currentTarget.position.z);
            if (!_runOnce) { _printStringCoroutine = StartCoroutine(Print(_currentDialogue)); }
            _runOnce = true;
        }
        else if (!_followObject && _enableChatter)
        { 
            if (_printStringCoroutine != null) { StopCoroutine(_printStringCoroutine); }
            _runOnce = false;
        }
        else if (!_followObject && !_enableChatter)
        {
            if (_chatterCoroutine != null) { StopCoroutine(_chatterCoroutine); } 
            if (_printStringCoroutine != null) { StopCoroutine(_printStringCoroutine); }
            _runOnce = false;
        }
    }

    IEnumerator ChatterEnabler()
    {
        while (_enableChatter)
        {
            _currentTarget = _npcTransforms[Random.Range(0, _npcTransforms.Length)];
            _currentDialogue = _chatterDialogues[Random.Range(0, _chatterDialogues.Length)];
            if (_currentTarget != null)
            {
                //Debug.Log(_currentDialogue);
                //Debug.Log(_currentTarget);

                _followObject = true;
            
                _text.text = "";
                transform.position = new Vector3(_currentTarget.position.x, _currentTarget.position.y + _heightAboveNPC, _currentTarget.position.z);

                _textObject.SetActive(true);
                yield return new WaitForSeconds(5f);
                _textObject.SetActive(false);
                // yield return new WaitForSeconds(1f);
                _followObject = false;
                yield return new WaitForSeconds(_timeBetweenChatter - 2f);
            }
            yield return null;
        }
    }

    IEnumerator Print(string _dialogueText)
    {
        _text.text = "";
        foreach (char c in _dialogueText)
        {
            _text.text += c;
            yield return new WaitForSeconds(0.04f);
        }
    }

   
}
