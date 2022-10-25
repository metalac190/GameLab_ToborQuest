using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public abstract class InteractablePad : MonoBehaviour {

    [Header("General Settings")]
    [SerializeField] private float padForceMaxTime;
    [SerializeField] private ParticleSystem particleVFX;
    [SerializeField] private SFXEvent audioSFX;
    [SerializeField] private UnityEvent onPlayerEnter;

    private Animator animator;

    abstract protected void OnRigidbodyTrigger(Rigidbody rb, ToborEffectsController effects);

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        //if the object has a rigidbody, call the pad's specific functionality
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb == null) {
            return;
        }

        //get the effect controller if there is one to pass into the specific pad
        ToborEffectsController effects = other.GetComponent<ToborEffectsController>();
        //call pad specific functionality
        OnRigidbodyTrigger(rb, effects);
        //spawn particles, play audio, and play animation
        if(particleVFX) StartCoroutine(Particles(other.gameObject.transform.position));
        if(audioSFX) audioSFX.Play();
        if(animator) animator.SetTrigger("Trigger");

        //if the object is Tobor, tell the movement control its using a pad and invoke any unity events
        MovementController movementController = other.GetComponent<MovementController>();
        if(movementController) onPlayerEnter?.Invoke();
    }

    private IEnumerator Particles(Vector3 spawnPosition) {
        var particles = Instantiate(particleVFX, spawnPosition, particleVFX.transform.rotation);
        yield return new WaitForSeconds(3); //wait arbitrary 3 seconds to delete particles effects just in case
        if(particles != null) Destroy(particles);
    }
}