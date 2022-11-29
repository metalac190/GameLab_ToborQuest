using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class InvisibleTrigger : MonoBehaviour
{
    [SerializeField, ReadOnly] private BoxCollider _collider;
    [SerializeField] private UnityEvent onPlayerEnter;
	[SerializeField] private Color32 gizmoColor = Color.green;
	[SerializeField] private bool onlyCalledOnce;

    private bool activatedFlag;

    private void Awake() {
        activatedFlag = false;
    }

    private void OnValidate() {
        if (!_collider) _collider = GetComponent<BoxCollider>();
        if (!_collider) _collider = gameObject.AddComponent<BoxCollider>();
    }

	private void OnTriggerEnter(Collider other) {
		if (activatedFlag && onlyCalledOnce) return;
		var player = other.GetComponent<MovementController>();
		if (player)
		{
			OnPlayerTrigger(player);
			activatedFlag = true;
			onPlayerEnter?.Invoke();
		}
    }

	protected virtual void OnPlayerTrigger(MovementController player) {
    }

    private void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        var t = transform;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(t.position, t.rotation, t.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(_collider.center, _collider.size);
    }
}