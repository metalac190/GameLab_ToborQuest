using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultPad : InteractablePad {

    [SerializeField] private Vector3 forceDirection;

    protected override void OnRigidbodyTrigger(Rigidbody rb) {
        //add an instant force in the indicated direction
        //rb.AddForce(addedForce * forceDirection, ForceMode.Impulse);

        //set the velocity to the catapult's force and direction
        rb.velocity = addedForce * forceDirection;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (transform.position + forceDirection));
    }
}