using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public class PushableButton : MonoBehaviour {

    [Header("Button Parts")]
    [SerializeField] private Transform buttonBase;
    [SerializeField] private Transform buttonTop;
    [SerializeField] private Transform buttonUpperLimit;
    [SerializeField] private Transform buttonLowerLimit;

    [Header("Button Settings")]
    [SerializeField, Range(0.1f, 1f)] private float thresholdPercentage;
    [SerializeField] private float buttonPopOutForce;
    [SerializeField] private float popOutWaitTime;
    [SerializeField] private bool buttonTogglable;

    [Header("VFX and SFX")]
    [SerializeField] private ParticleSystem particleVFXOnPush;
    [SerializeField] private ParticleSystem particleVFXOnRelease;
    [SerializeField] private SFXEvent audioSFXOnPush;
    [SerializeField] private SFXEvent audioSFXOnRelease;

    [Header("Button Flashing")]
    [SerializeField] private bool buttonFlashingOn;
    [SerializeField] private bool stopFlashingWhenPressed;
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashingInterval;
    [SerializeField] private MeshRenderer buttonTopMeshRenderer;
    
    [Header("Button After Pressed")]
    [SerializeField] private bool changeColorOnPress;
    [SerializeField] private Color pressedColor;

    [Header("Button Events")]
    [SerializeField] private UnityEvent onButtonToggleOn;
    [SerializeField] private UnityEvent onButtonToggleOff;
    [SerializeField, ReadOnly] private bool buttonActivated;

    private Rigidbody buttonTopRb;
    private float upperLowerDiff;
    private bool isPressed;
    private bool prevPressedState;

    private float buttonPressedTimer;
    private bool addButtonForce;

    private Material buttonTopMaterial;

    private void Awake() {
        buttonActivated = false;
        buttonTopRb = buttonTop.GetComponent<Rigidbody>();
        isPressed = false;
        buttonPressedTimer = 0;
        addButtonForce = false;

        buttonTopMaterial = buttonTopMeshRenderer.material;
    }

    private void Start() {
        //ignore collisions between the two button parts
        //(seems redundant with the button layers set up, but doesn't work without it)
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

        //start the flashing
        StartCoroutine(ButtonFlashing());
    }

    private void Update() {
        //lock the button top's local position to only go up and down, and clamp between limits
        float clampedY = Mathf.Clamp(buttonTop.localPosition.y, buttonLowerLimit.localPosition.y, 0);
        buttonTop.localPosition = new Vector3(0, clampedY, 0);

        if(buttonTop.localPosition.y < 0) {
            //if the button has been lower than the upper limit for 2 seconds, push it back up
            if(buttonPressedTimer >= popOutWaitTime) {
                addButtonForce = true;
            //else, increase the timer
            } else {
                addButtonForce = false;
                buttonPressedTimer += Time.deltaTime;
            }
        } else {
            buttonPressedTimer = 0;
            addButtonForce = false;
        }

        //set if the button is being pressed by checking if it is past the set threshold
        if(Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < (upperLowerDiff * thresholdPercentage)) {
            isPressed = true;
        } else {
            isPressed = false;
        }

        //check if the press button function should be called and call it
        if(isPressed && prevPressedState != isPressed) {
            PressButton();
            addButtonForce = false;
            buttonPressedTimer = 0;
        } else if(!isPressed && prevPressedState != isPressed) {
            ReleaseButton();
        }
    }

    private void FixedUpdate() {
        if(addButtonForce) {
            buttonTopRb.AddForce(buttonPopOutForce * Time.fixedDeltaTime * buttonTop.up);
        }
    }

    private void PressButton() {
        prevPressedState = isPressed;

        //play feedback
        if(particleVFXOnPush) StartCoroutine(Particles(particleVFXOnPush, buttonTop.position));
        if(audioSFXOnPush) audioSFXOnPush.Play();

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

        //turn off the button flashing since it has been pressed once
        if(stopFlashingWhenPressed) buttonFlashingOn = false;
        if (changeColorOnPress)
        {
            buttonTopMaterial.SetColor("_BaseColor", Color.Lerp(Color.white, pressedColor, 0.25f));
            buttonTopMaterial.SetColor("_EmissionColor", pressedColor);
        }
    }

    private void ReleaseButton() {
        prevPressedState = isPressed;

        //play feedback
        if(particleVFXOnRelease) StartCoroutine(Particles(particleVFXOnRelease, buttonTop.position));
        if(audioSFXOnRelease) audioSFXOnRelease.Play();
    }

    private IEnumerator Particles(ParticleSystem vfx, Vector3 spawnPosition) {
        var particles = Instantiate(vfx, spawnPosition, vfx.transform.rotation);
        yield return new WaitForSeconds(3); //wait arbitrary 3 seconds to delete particles effects just in case
        if(particles != null) Destroy(particles);
    }

    private IEnumerator ButtonFlashing() {
        while(buttonFlashingOn) {
            buttonTopMaterial.SetColor("_EmissionColor", flashColor);
            yield return new WaitForSeconds(flashingInterval);
            if (!buttonFlashingOn && changeColorOnPress) yield break;
            buttonTopMaterial.SetColor("_EmissionColor", Color.black);
            yield return new WaitForSeconds(flashingInterval);
        }
    }
}