using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    [SerializeField] public Transform _recentCheckpoint = null;
    [SerializeField] private float _respawnTime;
    [SerializeField] private GameObject _latestGround = null;
    [SerializeField] private GameObject _deathToborMarker = null;

    private Rigidbody _rb;
    private MovementController _mc;

    //GameObject _spawnedTobor;
    //private bool _isGroundedCheck;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mc = GetComponent<MovementController>();
    }

    private void Start()
    {
        var spawnPoint = Instantiate(new GameObject());
        spawnPoint.transform.position = gameObject.transform.position;
        spawnPoint.transform.rotation = gameObject.transform.rotation;
        spawnPoint.name = "Spawn Point";
        SetCheckpoint(spawnPoint.transform);

        //_latestGround = Instantiate(new GameObject());
        //_spawnedTobor = Instantiate(_deathToborMarker);
    }

    private void Update()
    {
        /*
        if (_isGroundedCheck != _mc.IsGrounded)
        {
            _isGroundedCheck = _mc.IsGrounded;
            if (!_mc.IsGrounded)
            {
                _latestGround.transform.position = transform.position;
                _latestGround.transform.rotation = transform.rotation;
                Debug.Log("Placed Latest Ground");
            }
        }
        */
    }

    private void OnDeath() 
    {
        
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
        //_spawnedTobor.transform.position = _latestGround.transform.position;
        //_spawnedTobor.transform.rotation = _latestGround.transform.rotation;
        yield return new WaitForSeconds(_respawnTime);
        mc.SetActive(true);
        transform.position = _recentCheckpoint.position;
        transform.rotation = _recentCheckpoint.rotation;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = false;
        _mc.SetBoostCharge(_mc.BoostChargeMax * 0.8f);
    }
}
