using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    [Header("Standard")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _wallBounceCooldown = 0.1f;
    [SerializeField] private bool _useVelocityBounce = true;

    [Header("Exaggerated Bounce Stats")]
    [SerializeField] private float _verticalBounce = 200f;
    [SerializeField] private float _horizontalBounce = 300f;
    [SerializeField] private bool _consistentBouncing = true;

    [Header("Velocity Bounce Stats")]
    [SerializeField] private Vector2 _velocityBounceMultiplier;

    private Rigidbody _rb;
    private MovementController _mc;
    private ToborEffectsController _toborEffects;

    private bool _canWallBounce = true;
    private Collision _currentWallCollision;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mc = GetComponent<MovementController>();
        _toborEffects = GetComponent<ToborEffectsController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _currentWallCollision = collision;
        if (_useVelocityBounce) VelocityWallBounce(collision.collider, collision.GetContact(0).normal);
        else ExaggeratedWallBounce(collision.collider, collision.GetContact(0).normal);
    }

    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == _currentWallCollision.gameObject)
        {
            //_canPlayImpactAnim = true;
        }
    } 
    

    private void ExaggeratedWallBounce(Collider otherCollider, Vector3 normal)
    {
        if ((_wallLayer.value & (1 << otherCollider.gameObject.layer)) <= 0) return;

        if (_canWallBounce)
        {
            _toborEffects.PlayOnCollision();
            //to stop boosting into wall
            if (_mc.UsingRechargeBoost) StartCoroutine(_mc.BoostRecharge());
            else StartCoroutine(_mc.BoostCooldown(0.1f));
            
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

    private void VelocityWallBounce(Collider otherCollider, Vector3 normal)
    {
        if ((_wallLayer.value & (1 << otherCollider.gameObject.layer)) <= 0) return;

        //effects
        _toborEffects.PlayOnCollision();

        if (_mc.UsingRechargeBoost) StartCoroutine(_mc.BoostRecharge());
        else StartCoroutine(_mc.BoostCooldown(0.25f));

        Vector3 vel = _rb.velocity;
        vel.y = 0;
        var perp = Vector2.Perpendicular(new Vector2(normal.x, normal.z));
        vel = Vector3.Project(vel, new Vector3(perp.x, 0, perp.y));
        _rb.velocity = vel;
        _rb.angularVelocity = Vector3.zero;

        var additionalForce = _mc.PreviousVelocity.magnitude * new Vector3(0, _velocityBounceMultiplier.y, 0);
        var bounceForce = normal * _mc.PreviousVelocity.magnitude * _velocityBounceMultiplier.x + additionalForce;

        _rb.AddForce(bounceForce, ForceMode.Impulse);
        //Debug.Log("Bounce Force: " + bounceForce);
        //Debug.Log("Additional Bounce: " + additionalForce);
        if (_wallBounceCooldown > 0) StartCoroutine(WallBounceCooldown());
    }
    
    private IEnumerator WallBounceCooldown()
    {
        _canWallBounce = false;
        yield return new WaitForSeconds(_wallBounceCooldown);
        _canWallBounce = true;
    }
}
