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
        SetCheckpoint(other.gameObject);
    }

    public void SetCheckpoint(GameObject otherObject)
    {
        var _checkpointTracker = otherObject.gameObject.GetComponent<CheckpointTracker>();
        if (_checkpointTracker != null) { _checkpointTracker._recentCheckpoint = UsedTransform; }
    }
}
