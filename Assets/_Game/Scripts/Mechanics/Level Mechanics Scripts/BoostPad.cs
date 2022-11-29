using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : InteractablePad {

    [Header("Boost Pad Settings")]
    [SerializeField] protected float addedForce;
    [SerializeField] private bool useVel;
	[SerializeField] private float boostVelocity;
	[SerializeField] private MeshRenderer _renderer;
	[SerializeField] private float _flashDuration = 1;
	
	private const string FlashKey = "_GradientColor";

    protected override void OnRigidbodyTrigger(Rigidbody rb, ToborEffectsController effects) {
        if(!useVel) {
            //add an instant force in the pad's forward direction
            rb.AddForce(addedForce * transform.forward, ForceMode.Impulse);
        } else {
            rb.velocity = boostVelocity * transform.forward;
        }
        //trigger the boost pad's effects on Tobor
	    if(effects) effects.PlayOnBoostPad();
	    StartCoroutine(FlashRoutine());
    }
    
	private IEnumerator FlashRoutine()
	{
		float speed = 1f / _flashDuration;
		var mat = _renderer.material;
		for (float t = 0; t < 1; t += Time.deltaTime * speed)
		{
			mat.SetFloat(FlashKey, t);
			yield return null;
		}
		mat.SetFloat(FlashKey, 0);
	}
}