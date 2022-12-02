using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMovement : MonoBehaviour
{
    Vector3 _startPos;
    Vector3 _endPos;
    public AnimationCurve MoveCruve;
    float _animationTimePos;
    float _speed;
    bool _movingLeft;

    private void Start()
    {
        _startPos = transform.position;
        _endPos = new Vector3 (transform.position.x-200, transform.position.y, transform.position.z);
        _movingLeft = true;
        _speed = Random.Range(0.0150f, 0.0275f);
    }

    // Update is called once per frame
    void Update()
    {
        cloudMove();
    }
    void cloudMove()
    {
        if (_movingLeft == false)
        {
            _animationTimePos += Time.deltaTime * _speed;
            transform.position = Vector3.Lerp(_endPos, _startPos, MoveCruve.Evaluate(_animationTimePos));
        }
        if (_movingLeft == true)
        {
            _animationTimePos += Time.deltaTime * _speed;
            transform.position = Vector3.Lerp(_startPos, _endPos, MoveCruve.Evaluate(_animationTimePos));
        }
        if (transform.position.x <= _endPos.x)
        {
            _movingLeft = false;
            transform.position = new Vector3(_endPos.x + 1f, _endPos.y, _endPos.z);
            _speed = Random.Range(0.01f, 0.03f);
            _animationTimePos = 0;
        }
        if (transform.position.x >= _startPos.x)
        {
            _movingLeft = true;
            transform.position = new Vector3(_startPos.x - 1f, _startPos.y, _startPos.z);
            _speed = Random.Range(0.01f, 0.03f);
            _animationTimePos = 0;
        }
    }

}
