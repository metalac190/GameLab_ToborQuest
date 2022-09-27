using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    
    private enum DoorType { Locked, Swinging, Sliding }
    [SerializeField] private DoorType doorType;

    private void Awake() {
        //lock door automatically
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ActivateDoor() {
        switch(doorType) {
            case DoorType.Locked:
                UnlockDoor();
                break;
            case DoorType.Swinging:
                SwingDoor();
                break;
            case DoorType.Sliding:
                SlideDoor();
                break;
            default:
                break;
        }
    }

    private void UnlockDoor() {
        //remove all constraints
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void SwingDoor() {
        UnlockDoor();
        //TODO: turn the door a certain amount
    }

    private void SlideDoor() {
        UnlockDoor();
        //TODO: slide the door a certain amount
    }
}