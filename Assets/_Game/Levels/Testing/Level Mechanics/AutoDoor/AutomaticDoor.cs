using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class AutomaticDoor : MonoBehaviour {

    [SerializeField] private bool startLocked;
    private Rigidbody rb;
    private HingeJoint hinge;

    private void Awake() {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();

        //lock door if startLocked is set
        if(startLocked) {
            LockDoor();
        }
        //turn off motor so that it can be used later
        hinge.useMotor = false;
    }

    public void LockDoor() {
        //apply all constraints to make sure nothing can move the door
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnlockDoor() {
        //remove all constraints to let physics take over
        rb.constraints = RigidbodyConstraints.None;
    }

    public void SwingDoor() {
        UnlockDoor();
        StartCoroutine(SwingingDoor());
    }

    //uses the pre-set motor velocity and force on the hinge joint component
    //only turns the motor on and off to allow the door to move on its own and then go back to normal (unlocked)
    private IEnumerator SwingingDoor() {
        hinge.useMotor = true;
        while(hinge.angle > hinge.limits.min && hinge.angle < hinge.limits.max) {
            yield return null;
        }
        hinge.useMotor = false;
    }
}