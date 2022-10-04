using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinField : MonoBehaviour
{
    [SerializeField] LayerMask _desiredLayer;
    [SerializeField] BoxCollider _collider;

    void OnValidate()
    {
        if (_collider == null) { _collider = GetComponent<BoxCollider>(); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _desiredLayer)
        {
            OnWin();
        }
    }

    void OnWin()
    {
        CGSC.WinGame();
    }

     void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _collider.size);
        
    }
}
