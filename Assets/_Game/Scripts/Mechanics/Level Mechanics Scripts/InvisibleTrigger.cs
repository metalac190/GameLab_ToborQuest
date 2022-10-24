using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class InvisibleTrigger : MonoBehaviour {

    [SerializeField] private UnityEvent onPlayerEnter;
    [SerializeField] private Color32 gizmoColor = Color.green;

    private bool activatedFlag;

    private void Awake() {
        activatedFlag = false;
    }

    private void OnTriggerEnter(Collider other) {
        OnPlayerTrigger(other);
    }

    virtual protected void OnPlayerTrigger(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && !activatedFlag) {
            activatedFlag = true;
            onPlayerEnter?.Invoke();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        Vector3 center = transform.position + GetComponent<BoxCollider>().center;
        Vector3 size = GetComponent<BoxCollider>().size;
        Gizmos.DrawWireCube(center, size);
    }
}