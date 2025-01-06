using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject wizardPrefab;  // The wizard prefab to spawn
    [SerializeField] private Transform enemiesParent;  // Parent object for spawned enemies

    private int wizardCount = 0;  // Track the number of wizards spawned

    public Waypoints waypoints;  // Reference to the Waypoints system
    public float moveSpeed = 5f;  // Movement speed for the spawned wizard

    // Call this function when you want to spawn a wizard
    public void SpawnWizard()
    {
        // Instantiate the wizard prefab at the spawner's position with no rotation
        GameObject wizard = Instantiate(wizardPrefab, transform.position, Quaternion.identity);
        wizard.transform.SetParent(enemiesParent);  // Set the parent of the spawned wizard

        // Name the wizard (e.g., "Wizard 1")
        wizard.name = wizardPrefab.name + " " + (wizardCount + 1);
        wizardCount++;  // Increment the wizard count

        // Start the movement for the spawned wizard
        StartMovingWizard(wizard);
    }

    // This method handles the movement of the spawned wizard through the waypoints
    private void StartMovingWizard(GameObject wizard)
    {
        StartCoroutine(MoveThroughWaypoints(wizard));
    }

    private IEnumerator MoveThroughWaypoints(GameObject wizard)
    {
        Transform currentWaypoint = waypoints.GetNextWaypoint(null);  // Get first waypoint
        float distanceThreshold = 0.1f;  // Distance to consider as "reached"

        while (currentWaypoint != null)
        {
            if (wizard == null) yield break;

            // Move the wizard towards the current waypoint
            wizard.transform.position = Vector3.MoveTowards(wizard.transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

            // Check if the wizard has reached the waypoint
            if (Vector3.Distance(wizard.transform.position, currentWaypoint.position) < distanceThreshold)
            {
                // Move to the next waypoint
                currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            }

            // Wait for the next frame before continuing the loop
            yield return null;
        }

        // Optionally, handle what happens when the wizard reaches the final waypoint
        // For example, damage the stronghold or destroy the wizard
        if (Vector3.Distance(wizard.transform.position, waypoints.stronghold.transform.position) < 2.5f) {
            HealthBarController.instance.TakeDamage(10f);  // Example damage
            Destroy(wizard);  // Destroy the wizard
        }
    }
}