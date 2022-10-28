using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Much of these calculations and functions are taken from IronWarrior's ProjectileShooting project on github, linked here:
 * https://github.com/IronWarrior/ProjectileShooting
 */

public class CatapultPad : InteractablePad {

    [Header("Catapult Settings")]
    [SerializeField] private float launchAngle;
    [SerializeField] private Transform catapultBase;
    [SerializeField] private Transform catapultLauncher;
    [SerializeField] private Transform art;
    [SerializeField] private Transform targetTransform;
    [SerializeField, ReadOnly] private float launchSpeed;
    [SerializeField, ReadOnly] private float flightTime;

    private Vector3 arcDirection;
    private float arcDistance;
    private float launchAngleRadians;
    
    private void Awake() {
        Vector3 artLookAt = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        art.LookAt(artLookAt);
        SetCatapultArc();
    }

    protected override void OnRigidbodyTrigger(Rigidbody rb, ToborEffectsController effects) {
        //start a coroutine to disable custom gravity and drag during flight time
        StartCoroutine(DisableOddities(rb));
        //set the object's velocity to the 3D launch vector times the calculated launch speed
        rb.angularVelocity = Vector3.zero;
        rb.velocity = catapultLauncher.up * launchSpeed;
        //trigger the catapult's effects on Tobor
        if(effects) effects.PlayOnCatapult();
    }

    //disable custom gravity, movement controls, and drag on Tobor
    private IEnumerator DisableOddities(Rigidbody rb)
    {
        GravityController grav = rb.GetComponent<GravityController>();
        MovementController move = rb.GetComponent<MovementController>();
        float originalDrag = rb.drag;

        if(rb) rb.drag = 0;
        if(grav) grav.GravityEnabled = false;
        if(move) move.SetActive(false);

        //waits until Tobor is grounded, or the time has run out, whichever happens first
        yield return new WaitForSeconds(0.1f); //buffer for wheels to get off the ground
        float inAirTimer = 0;
        if(move) {
            while(!move.GroundCheck() && inAirTimer < flightTime) {
                inAirTimer += Time.deltaTime;
                yield return null;
            }
        } else {
            while(inAirTimer < flightTime) {
                inAirTimer += Time.deltaTime;
                yield return null;
            }
        }
        

        if(grav) grav.GravityEnabled = true;
        if(move) move.SetActive(true);
        if(rb) rb.drag = originalDrag;
    }

    /*
     * Right now, this assumes that the launch speed is calculated based on the given launch angle.
     * An option can later be added to calculate the launch angle based on the given launch speed, but that is less useful for a few reasons.
     */
    private void SetCatapultArc() {
        arcDirection = targetTransform.position - transform.position; //get difference vector between catapult and target
        float yOffset = -arcDirection.y;
        arcDirection -= Vector3.Dot(arcDirection, Vector3.up) * Vector3.up; //project direction vector onto the Up plane
        arcDistance = arcDirection.magnitude; //magnitude of the straight line between end points

        float gravity = Physics.gravity.magnitude;

        //calculate the necessary launch speed to make the object move along the desired curve using physics
        launchAngleRadians = launchAngle * Mathf.Deg2Rad;
        launchSpeed = (arcDistance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(launchAngleRadians))) / Mathf.Sqrt(2 * arcDistance * Mathf.Sin(launchAngleRadians) + 2 * yOffset * Mathf.Cos(launchAngleRadians));

        catapultBase.rotation = Quaternion.LookRotation(arcDirection) * Quaternion.Euler(-90, -90, 0); //rotate Base object to rotate launch vector on the y axis
        catapultLauncher.localRotation = Quaternion.Euler(90, 90, 0) * Quaternion.AngleAxis(launchAngle, Vector3.forward); //rotate Launcher object to rotate the launch vector up and down

        float ySpeed = launchSpeed * Mathf.Sin(launchAngleRadians);
        flightTime = (ySpeed + Mathf.Sqrt((ySpeed * ySpeed) + 2 * gravity * yOffset)) / gravity;
    }

    private Vector3[] MakeArcPoints() {
        int iterations = 20;
        
        float iterationSize = arcDistance / iterations;
        Vector3[] points = new Vector3[iterations + 1];

        Vector3 linearVector = targetTransform.position - transform.position;
        linearVector = Vector3.Normalize(linearVector);

        for(int i = 0; i <= iterations; i++) {
            //set the step with the arbitrary iteration size already set
            float step = iterationSize * i;

            //get y positions for points along the curve
            float t = step / (launchSpeed * Mathf.Cos(launchAngleRadians));
            float y = (-0.5f * Physics.gravity.magnitude * (t * t) + launchSpeed * Mathf.Sin(launchAngleRadians) * t) + transform.position.y;

            //get the x and z positions with the straight line from end to end
            Vector3 xzLine = transform.position + (step * linearVector);
            float x = xzLine.x;
            float z = xzLine.z;

            //create new point in the air along the curve and add it to the array
            Vector3 p = new Vector3(x, y, z);
            points[i] = p;
        }

        return points;
    }

    private void OnDrawGizmosSelected() {
        //calculate the catapult's launch arc based on the target position and launch angle
        SetCatapultArc();
        Vector3[] arcPoints = MakeArcPoints();

        //display desired travel arc with gizmos
        Gizmos.color = Color.yellow;
        for(int i = 0; i < arcPoints.Length-1; i++) {
            Gizmos.DrawLine(arcPoints[i], arcPoints[i + 1]);
        }
    }    
}