using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    [SerializeField] public Transform _recentCheckpoint = null;
    [SerializeField] private float _respawnTime;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

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
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        gameObject.transform.position = _recentCheckpoint.position + new Vector3(0, 5, 0);
    }
}
