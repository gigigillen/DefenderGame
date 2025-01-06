using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour {
    // gets the wizard and its parent path
    public GameObject wizardPrefab;  
    [SerializeField] private Transform enemiesParent;  

    private int wizardCount = 0;  

    public Waypoints waypoints; 
    public float moveSpeed = 5f;  

    // spawns the wizard (duh)
    public void SpawnWizard()
    {
        // spawns a wizard without any coroutines
        GameObject wizard = Instantiate(wizardPrefab, transform.position, Quaternion.identity);
        wizard.transform.SetParent(enemiesParent);  

        wizard.name = wizardPrefab.name + " " + (wizardCount + 1);
        wizardCount++;  // Increment the wizard count

        // start the movement for the spawned wizard
        StartMovingWizard(wizard);
    }

    // this method handles the movement of the spawned wizard through the waypoints
    private void StartMovingWizard(GameObject wizard)
    {
        StartCoroutine(MoveThroughWaypoints(wizard));
    }

    private IEnumerator MoveThroughWaypoints(GameObject wizard)
    {
        Transform currentWaypoint = waypoints.GetNextWaypoint(null); 
        float distanceThreshold = 0.1f;  

        while (currentWaypoint != null)
        {
            if (wizard == null) yield break;

            // move the wizard towards the current waypoint
            wizard.transform.position = Vector3.MoveTowards(wizard.transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

            // check if the wizard has reached the waypoint
            if (Vector3.Distance(wizard.transform.position, currentWaypoint.position) < distanceThreshold)
            {
                // move to the next waypoint
                currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            }

            // wait for the next frame before continuing the loop
            yield return null;
        }

        // Destroys the wizard when it's health reaches 0
        if (Vector3.Distance(wizard.transform.position, waypoints.stronghold.transform.position) < 2.5f) {
            HealthBarController.instance.TakeDamage(10f);  
            Destroy(wizard);  
        }
    }
}