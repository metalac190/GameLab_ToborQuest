using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    public Vector3[] positions;
    public float platformSpeed;
    private Rigidbody _rigidbody;
    public MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private int currentPosition = 0;
    private Vector3 startPos;

    //Allows gizmos to draw correctly
    private void OnValidate()
    {
        positions[0] = this.transform.position;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(currentPosition);
        if (Vector3.Distance(transform.position, positions[currentPosition]) > 0.1f)
        {
            _rigidbody.MovePosition(_rigidbody.position + (positions[currentPosition] - this.transform.position).normalized * platformSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (currentPosition + 1 >= positions.Length)
            {
                currentPosition = 0;
            }
            else
            {
                currentPosition++;
            }
        }         
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < positions.Length; i++)
        {
            Gizmos.DrawWireMesh(meshFilter.sharedMesh,-1,positions[i],transform.rotation,transform.lossyScale);
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(positions[0], positions[1]);
        for (int i = 1; i < positions.Length - 1; i++)
        {
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
        Gizmos.DrawLine(positions[positions.Length-1], positions[0]);
    }
}
