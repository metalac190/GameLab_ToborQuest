using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnField : MonoBehaviour
{
    [SerializeField] Transform _spawnTransform;
    [SerializeField] BoxCollider _collider;

    void OnValidate()
    {
        if (_collider == null) { _collider = GetComponent<BoxCollider>(); }
    }
    Transform UsedTransform =>(_spawnTransform != null) ? _spawnTransform : transform;
    void OnTriggerEnter(Collider other)
    {
        var _checkpointTracker = other.gameObject.GetComponent<CheckpointTracker>();
        if (_checkpointTracker != null) { _checkpointTracker._recentCheckpoint = UsedTransform; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, _collider.size);
        Gizmos.DrawWireSphere(_spawnTransform.position, 1f);
    }
}
