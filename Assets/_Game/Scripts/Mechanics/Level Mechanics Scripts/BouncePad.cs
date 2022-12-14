using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : InteractablePad {

    [Header("Bounce Pad Settings")]
    [SerializeField] protected float addedForce;

    protected override void OnRigidbodyTrigger(Rigidbody rb, ToborEffectsController effects) {
        //add an instant force in the pad's upwards direction
        rb.AddForce(addedForce * transform.up, ForceMode.Impulse);
        //trigger the bounce pad's effects on Tobor
        if(effects) effects.PlayOnBouncePad();
    }
}