using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour {

    [SerializeField] private bool buttonTogglable;
    [SerializeField] private UnityEvent eventOnButtonActivate;
    [SerializeField] private UnityEvent eventOnButtonDeactivate;
    [SerializeField, ReadOnly] private bool buttonActivated;

    private bool flag;

    private void Awake() {
        buttonActivated = false;
        flag = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(flag) {
            return;
        }

        flag = true;
        if(other.gameObject.CompareTag("Player")) {
            Debug.Log("Player touched button");
            //set buttonActivated based on settings
            if(buttonTogglable) {
                buttonActivated = !buttonActivated;
            } else {
                buttonActivated = true;
            }

            //invoke the appropriate event
            if(buttonActivated) {
                eventOnButtonActivate?.Invoke();
            } else {
                eventOnButtonDeactivate?.Invoke();
            }
        }
        flag = false;
    }
}