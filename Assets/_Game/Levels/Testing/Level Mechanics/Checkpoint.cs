using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{

    [SerializeField] private List<GameObject> objectsToDelete = new List<GameObject>();
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
