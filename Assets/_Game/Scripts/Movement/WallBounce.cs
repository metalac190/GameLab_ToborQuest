using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    [Header("Wall Bounce")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _verticalBounce = 200f;
    [SerializeField] private float _horizontalBounce = 300f;

    private Rigidbody _rb;
    private MovementController _mc;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _mc = GetComponentInParent<MovementController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ExaggeratedWallBounce(collision.collider, collision.GetContact(0).normal);
    }

    private void ExaggeratedWallBounce(Collider otherCollider, Vector3 normal)
    {
        if ((_wallLayer.value & (1 << otherCollider.gameObject.layer)) <= 0) return;

        //to stop boosting into wall
        StartCoroutine(_mc.BoostCooldown(0.1f));
        _rb.AddForce(normal * _horizontalBounce + new Vector3(0, _verticalBounce, 0), ForceMode.Impulse);
    }
}
