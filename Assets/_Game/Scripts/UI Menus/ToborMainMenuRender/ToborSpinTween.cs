using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborSpinTween : MonoBehaviour
{
    [SerializeField]public float spinRate = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * spinRate);
    }

}
