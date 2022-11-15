using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMeshOnStart : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    private void Start()
    {
        _renderer.enabled = true;
    }
}
