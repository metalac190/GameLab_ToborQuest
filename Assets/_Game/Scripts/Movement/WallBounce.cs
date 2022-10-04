using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    [Header("Wall Bounce")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _verticalBounce = 200f;
    [SerializeField] private float _horizontalBounce = 300f;
    [SerializeField] private float _wallBounceCooldown = 0.1f;
    [SerializeField] private bool _consistentBouncing = true;

    private Rigidbody _rb;
    private MovementController _mc;
    private bool _canWallBounce = true;

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

        
        if (_canWallBounce)
        {
            //to stop boosting into wall
            StartCoroutine(_mc.BoostCooldown(0.1f));
            
            if (_consistentBouncing)
            {
                Vector3 vel = _rb.velocity;
                vel.y = 0;
                var perp = Vector2.Perpendicular(new Vector2(normal.x, normal.z));
                vel = Vector3.Project(vel, new Vector3(perp.x, 0, perp.y));
                _rb.velocity = vel;
                _rb.angularVelocity = Vector3.zero;
            }
            _rb.AddForce(normal * _horizontalBounce + new Vector3(0, _verticalBounce, 0), ForceMode.Impulse);
            if (_wallBounceCooldown > 0) StartCoroutine(WallBounceCooldown());
        }
    }
    
    private IEnumerator WallBounceCooldown()
    {
        _canWallBounce = false;
        yield return new WaitForSeconds(_wallBounceCooldown);
        _canWallBounce = true;
    }
}
