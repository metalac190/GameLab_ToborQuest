using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractablePad : MonoBehaviour {

    [SerializeField] protected float addedForce;
    [SerializeField] private float padForceMaxTime;

    //static timer to be shared by all pads
    private static float padTimer;

    abstract protected void OnRigidbodyTrigger(Rigidbody rb);

    private void Awake() {
        padTimer = 0;
    }

    private void Update() {
        padTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        //if the object has a rigidbody, call the pad's specific functionality
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb == null) {
            return;
        }
        OnRigidbodyTrigger(rb);

        //if the object is Tobor, call the coroutine to set its boost pad boolean to turn off max speed
        MovementController movementController = other.GetComponent<MovementController>();
        if(movementController != null) {
            StartCoroutine(BoostPadTimer(movementController));
        }
    }

    private IEnumerator BoostPadTimer(MovementController movementController) {
        movementController._UsingPad = true;
        padTimer = 0;

        yield return new WaitUntil(() => padTimer >= padForceMaxTime);

        //shouldn't trigger until Tobor hasn't touched ANY pad for padForceMaxTime time
        movementController._UsingPad = false;
    }
}