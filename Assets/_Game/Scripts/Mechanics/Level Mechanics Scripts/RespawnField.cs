using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnField : InvisibleTrigger
{
    #region Do not read...trust me...
    // [SerializeField] Transform _spawnTransform;
    // [SerializeField] BoxCollider _collider;

    // void OnValidate()
    // {
    //     if (_collider == null) { _collider = GetComponent<BoxCollider>(); }
    // }
    // Transform UsedTransform =>(_spawnTransform != null) ? _spawnTransform : transform;
    // void OnTriggerEnter(Collider other)
    // {
    //     SetCheckpoint(other.gameObject);
    // }

    // public void SetCheckpoint(GameObject otherObject)
    // {
    //     var _checkpointTracker = otherObject.gameObject.GetComponent<CheckpointTracker>();
    //     if (_checkpointTracker != null) { _checkpointTracker._recentCheckpoint = UsedTransform; }
    // }
    #endregion

    [SerializeField] bool _respawnAtCheckpoint = false;

    protected override void OnPlayerTrigger(Collider other)
    {
        var _checkpointTracker = other.gameObject.GetComponent<CheckpointTracker>();

       if (_respawnAtCheckpoint && _checkpointTracker != null)
       {
             _checkpointTracker.Respawn();
             Debug.Log($"[RESPAWN FIELD] Respawned!");
       }
       else if (!_respawnAtCheckpoint && _checkpointTracker != null)
       {
             _checkpointTracker.SetCheckpoint(transform);
             Debug.Log($"[RESPAWN FIELD] Set the checkpoint at {transform.position}");
       }
        base.OnPlayerTrigger(other);
    }
}
