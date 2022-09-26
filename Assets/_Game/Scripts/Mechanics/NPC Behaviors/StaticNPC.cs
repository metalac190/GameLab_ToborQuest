using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StaticNPC : MonoBehaviour
{
    //This is just a base class incase both types of NPCs share features.
    [SerializeField] protected Rigidbody _rB;
    [SerializeField] protected bool _upAndDownMotion = true;
   [SerializeField] protected float _bobSpeed = 1f;
   [SerializeField] protected float _bobAmplitude = 1f;

   protected Vector3 _defaultPos;


    private void OnValidate()
    {
        if (_rB == null) {_rB = GetComponent<Rigidbody>();}
    }

    
  

   private void Awake()
   {
        _defaultPos = transform.position;
   }


   private void FixedUpdate()
   {
        if (_upAndDownMotion)
        {
            _rB.MovePosition(new Vector3(
                            _defaultPos.x, 
                            (Mathf.Sin(Time.time * _bobSpeed) * _bobAmplitude) + _defaultPos.y, 
                            _defaultPos.z)
                        );
        }
        
   }
}
