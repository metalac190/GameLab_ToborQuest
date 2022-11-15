using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMovement : MonoBehaviour
{
    float _moveSpeed;
    float _startSpeed;
    Vector3 _startPos;
    Vector3 _endPos;

    private void Start()
    {
        _startPos = transform.position;
        _endPos = new Vector3 (transform.position.x-200, transform.position.y, transform.position.z);
        _moveSpeed = Random.Range(8, 12);
        _startSpeed = _moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //cloudMove();
    }

    void cloudMove()
    {
        if (transform.position.x <= _endPos.x)
        {
            StartCoroutine(decelearte(1));
            transform.position = new Vector3(_endPos.x + .1f, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= _startPos.x)
        {
            StartCoroutine(decelearte(-1));
            transform.position = new Vector3(_startPos.x - .1f, transform.position.y, transform.position.z);
        }
        transform.position = transform.position + new Vector3(_moveSpeed * Time.deltaTime, 0, 0);
    }

    /*
    IEnumerator decelearte(float directionNum)
    {
        float setupSpeed = _moveSpeed;
        for (float speed = setupSpeed; speed >= 0; speed -= 0.1f)
        {
            _moveSpeed = speed;
            yield return null;
        }
        StartCoroutine(accelerate(directionNum));
    }

    IEnumerator accelerate(float directionNum)
    {
        _startSpeed = directionNum;
        for (float speed = 0; speed <= _startSpeed; speed += 0.1f)
        {
            _moveSpeed = speed;
            yield return null;
        }
    }
    */
}
