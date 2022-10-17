using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    [SerializeField] public Transform _recentCheckpoint = null;
    [SerializeField] private float _respawnTime;

    private void Start()
    {
        var spawnPoint = Instantiate(new GameObject());
        spawnPoint.transform.position = gameObject.transform.position;
        spawnPoint.transform.rotation = gameObject.transform.rotation;
        spawnPoint.name = "Spawn Point";
        SetCheckpoint(spawnPoint.transform);
    }

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
