using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborSpinTween : MonoBehaviour
{
    [SerializeField] private float spinRate = 5f;
    [SerializeField] private List<Transform> _transformsToSpin = new List<Transform>();

    void Update()
    {
        foreach (var t in _transformsToSpin)
        {
            t.Rotate(Vector3.up, Time.deltaTime * spinRate);
        }
    }

}
