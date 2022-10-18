using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : InteractablePad {

    [Header("Boost Pad Settings")]
    [SerializeField] protected float addedForce;
    [SerializeField] private bool useVel;
    [SerializeField] private float boostVelocity;

    protected override void OnRigidbodyTrigger(Rigidbody rb) {
        if(!useVel) {
            //add an instant force in the pad's forward direction
            rb.AddForce(addedForce * transform.forward, ForceMode.Impulse);
        } else {
            rb.velocity = boostVelocity * transform.forward;
        }
        
    }
}