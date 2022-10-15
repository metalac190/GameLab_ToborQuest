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
        //call pad specific functionality
        OnRigidbodyTrigger(rb);
        //spawn particles
        if(particleVFX != null) StartCoroutine(Particles(other.gameObject.transform.position));
        //play audio
        audioSFX?.Play();

        //if the object is Tobor, tell the movement control its using a pad and invoke any unity events
        MovementController movementController = other.GetComponent<MovementController>();
        if(movementController != null) {
            StartCoroutine(PadTimer(movementController));
            onPlayerEnter?.Invoke();
        }
    }

    private IEnumerator PadTimer(MovementController movementController) {
        movementController._UsingPad = true;
        padTimer = 0;

        yield return new WaitUntil(() => padTimer >= padForceMaxTime);

        //shouldn't trigger until Tobor hasn't touched ANY pad for padForceMaxTime time
        movementController._UsingPad = false;
    }

    private IEnumerator Particles(Vector3 spawnPosition) {
        var particles = Instantiate(particleVFX, spawnPosition, particleVFX.transform.rotation);
        yield return new WaitForSeconds(3); //wait arbitrary 3 seconds to delete particles effects just in case
        if(particles != null) Destroy(particles);
    }
}