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
        var mc = GetComponent<MovementController>();
        mc.SetActive(false);
        _rb.isKinematic = true;
        yield return new WaitForSeconds(_respawnTime);
        mc.SetActive(true);
        transform.position = _recentCheckpoint.position;
        transform.rotation = _recentCheckpoint.rotation;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = false;
    }
}
