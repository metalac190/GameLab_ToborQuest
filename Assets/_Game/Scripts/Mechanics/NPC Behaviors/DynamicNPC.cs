using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicNPC : StaticNPC
{
    [SerializeField, HighlightIfNull] private Waypoints _waypoints;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _distanceThreshold = 0.1f;
    
    [SerializeField] bool _canMove = true;
    private Transform _currentWaypoint;

  
    private void Start()
    {
       if (_waypoints != null)
       {
            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
            transform.position = _currentWaypoint.position;

            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);

            _defaultPos = transform.position;
       }
    }


    private void Update() 
    {
        //TODO: Change this to rely on Rigidbody Movement instead of Transform Position

        Vector3 mov = new Vector3 (transform.position.x, Mathf.Sin(_bobSpeed * Time.time) * _bobAmplitude + _defaultPos.y, transform.position.z);
        transform.position = mov;
    }


    private void FixedUpdate()
    {
        if (_canMove && _waypoints != null)
        {
            _rB.MovePosition(Vector3.MoveTowards(transform.position, _currentWaypoint.position, _moveSpeed * Time.deltaTime));
            if (Vector3.Distance(transform.position, _currentWaypoint.position) < _distanceThreshold)
            {   
                _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
            }
        }
    }
    
}
