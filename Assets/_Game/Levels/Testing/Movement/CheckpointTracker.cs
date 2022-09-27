using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    [SerializeField] public Transform _recentCheckpoint = null;
    [SerializeField] private float _respawnTime;

    public void SetCheckpoint(Transform checkpoint)
    {
        _recentCheckpoint = checkpoint;
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        gameObject.transform.position = _recentCheckpoint.position;
    }
}
