using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : InteractablePad {

    protected override void OnRigidbodyTrigger(Rigidbody rb) {
        //add an instant force in the pad's forward direction
        rb.AddForce(addedForce * transform.forward, ForceMode.Impulse);
    }
}