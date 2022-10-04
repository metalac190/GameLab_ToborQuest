using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{

    [SerializeField] private List<GameObject> objectsToDelete = new List<GameObject>();
    

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MovementController>())
        {
            //Deletes the List of Objects
            foreach (GameObject i in objectsToDelete)
            {
                Destroy(i);
            }
        }
    }
}
