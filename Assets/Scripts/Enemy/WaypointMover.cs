using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    //stores a ref to the waypoint system
    public Waypoints waypoints;

    //default speed
    public float moveSpeed = 5f;

    //where the enemy is moving too
    private Transform currentWaypoint;

    public float distanceThreshold = 0.1f;

    public GameObject stronghold;


    // Start is called before the first frame update
    void Start()
    {
        //set initial position to first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //set next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        
    }

    // Update is called once per frame
    void Update()
    {

        // ensures the enemies attack on a fixed y position
        Vector3 targetPosition = stronghold.transform.position;
        targetPosition.y = transform.position.y;


        //moves enemy from point to point
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);


        //keep moving
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }

        if (Vector3.Distance(transform.position, stronghold.transform.position) < 2.5)
        {
            HealthBarController.instance.TakeDamage(10f);
            Destroy(gameObject);
        }

    }
}
