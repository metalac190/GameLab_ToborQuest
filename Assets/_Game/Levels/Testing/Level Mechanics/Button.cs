using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour {

    [Header("Button Parts")]
    [SerializeField] private Transform buttonBase;
    [SerializeField] private Transform buttonTop;
    [SerializeField] private Transform buttonUpperLimit;
    [SerializeField] private Transform buttonLowerLimit;

    [Header("Button Settings")]
    [SerializeField] private float thresholdPercentage; //between 0 and 1
    [SerializeField] private float buttonForce;
    [SerializeField] private bool buttonTogglable;

    [Header("Button Events")]
    [SerializeField] private UnityEvent onButtonToggleOn;
    [SerializeField] private UnityEvent onButtonToggleOff;
    [SerializeField, ReadOnly] private bool buttonActivated;

    private float upperLowerDiff;
    private bool isPressed;
    private bool prevPressedState;

    private void Awake() {
        buttonActivated = false;
    }

    private void Start() {
        //ignore collisions between the two button parts
        Physics.IgnoreCollision(buttonBase.GetComponent<Collider>(), buttonTop.GetComponent<Collider>());

        //get the distance between the upper and lower limits
        if(buttonBase.transform.eulerAngles != Vector3.zero) {
            Vector3 savedAngle = buttonBase.transform.eulerAngles;
            buttonBase.transform.eulerAngles = Vector3.zero;
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
            buttonBase.transform.eulerAngles = savedAngle;
        } else {
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
        }
    }

    private void Update() {
        //lock the button top's local position and rotation to only go up and down
        buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
        //buttonTop.transform.localEulerAngles = Vector3.zero;

        //clamp the button's highest position to the upper limit
        if(buttonTop.localPosition.y > 0) {
            buttonTop.transform.position = buttonUpperLimit.transform.position;
        //if the button is lower than the upper limit, then add a spring-like force upward
        } else {
            buttonTop.GetComponent<Rigidbody>().AddForce(buttonTop.transform.up * buttonForce * Time.fixedDeltaTime);
        }

        //clamp the button's lowest point to the lower limit
        if(buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y) {
            buttonTop.transform.position = buttonLowerLimit.transform.position;
        }

        //set if the button is being pressed by checking if it is past the set threshold
        if(Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * thresholdPercentage) {
            isPressed = true;
        } else {
            isPressed = false;
        }

        //check if the press button function should be called and call it
        if(isPressed && prevPressedState != isPressed) {
            PressButton();
        } else if(!isPressed && prevPressedState != isPressed) {
            ReleaseButton();
        }
    }

    private void PressButton() {
        prevPressedState = isPressed;

        //TODO: add feedback

        //set buttonActivated based on settings
        if(buttonTogglable) {
            buttonActivated = !buttonActivated;
        } else {
            buttonActivated = true;
        }

        //invoke the appropriate event
        if(buttonActivated) {
            onButtonToggleOn?.Invoke();
        } else {
            onButtonToggleOff?.Invoke();
        }
    }

    private void ReleaseButton() {
        prevPressedState = isPressed;

        //TODO: add feedback maybe?
    }
}