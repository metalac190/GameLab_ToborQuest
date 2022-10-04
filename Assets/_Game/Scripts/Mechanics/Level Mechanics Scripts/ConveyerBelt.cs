using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour {

    [SerializeField] private float speed; //it is recommended not to put this too high or else collision can get weird
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 pos = rb.position;
        rb.position += -transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}