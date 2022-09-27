using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class InvisibleTrigger : MonoBehaviour {

    [SerializeField] private UnityEvent onPlayerEnter;

    private bool activatedFlag;

    private void Awake() {
        activatedFlag = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !activatedFlag) {
            activatedFlag = true;
            onPlayerEnter?.Invoke();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + GetComponent<BoxCollider>().center;
        Vector3 size = GetComponent<BoxCollider>().size;
        Gizmos.DrawWireCube(center, size);
    }
}