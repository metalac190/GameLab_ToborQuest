using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoints : MonoBehaviour
{
    [Range(0.1f, 1f)]
    [SerializeField] float _waypointSize = 1f;
    [SerializeField] float _randomScatter = 20f;
 

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_waypointSize, _waypointSize, _waypointSize));

        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, _waypointSize);
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        Gizmos.DrawLine(
                transform.GetChild(transform.childCount - 1).position, 
                transform.GetChild(0).position);
        
    }



    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)                                            //<-------If the waypoint is outside of possible values
        {
            return transform.GetChild(0);
        }
        
        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)       //<--------if the waypoint is anything but the last waypoint
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else                                                                    //<----------if the waypoint is the last waypoint
        {
            return transform.GetChild(0);
        }
    }

    [Button(Mode = ButtonMode.NotInPlayMode)]
    public void ResetWaypointPosition()
    {
        foreach (Transform t in transform)
        {
            t.position = transform.position;
        }
    }

    [Button(Mode = ButtonMode.NotInPlayMode)]
    public void RandomWaypointScatter()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).position = new Vector3(Random.Range(0, _randomScatter), transform.position.y, Random.Range(0, _randomScatter));
        }
    }
}
