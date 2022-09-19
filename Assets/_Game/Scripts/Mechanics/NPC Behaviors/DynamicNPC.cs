using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicNPC : StaticNPC
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    [SerializeField] bool _canMove = true;
    [SerializeField] float _moveDuration = 3f;
    
    private float _elapsedTime = 0f;
    private Vector3 _startingPos;
    private Transform _currentTargetWayPoint;


    private void Start()
    {
        _startingPos = transform.position;
        _currentTargetWayPoint = transform;
    }


    private void OnDrawGizmosSelected()
    {
        if (waypoints.Count > 0)
        {
            Gizmos.DrawLine(transform.position, waypoints[0].position);
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }


    private void FixedUpdate()
    {
       if (waypoints.Count > 0)
       {
            for (int i = 1; i < waypoints.Count; i++)
            {
                _currentTargetWayPoint = waypoints[i];
                if (_canMove)
                    {
                        _elapsedTime += Time.deltaTime;
                        float _movementPercentage = _elapsedTime / _moveDuration;

                        _rB.MovePosition(
                            Vector3.Lerp(transform.position, 
                            _currentTargetWayPoint.position, 
                            _movementPercentage));
                    }
            }
       }
    }
}
