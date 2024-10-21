using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{

    private int damage = 1;
    private Health targetedEnemyHealth;
    private SpawnApprentice spawnApprentice;

    private void Awake() {
        spawnApprentice = FindAnyObjectByType<SpawnApprentice>();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Health>() != null && targetedEnemyHealth == null)
        {
            targetedEnemyHealth = collider.GetComponent<Health>();
            targetedEnemyHealth.Damage(damage);
            Debug.Log("Dealt " + damage + " damage to enemy");

            // Check if the enemy is defeated
            if (targetedEnemyHealth.health <= 0)
            {
                // Destroy the apprentice after defeating the enemy
                Destroy(transform.parent.gameObject);
                spawnApprentice.killApprentice();

            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Clear the targeted enemy when it leaves the attack area
        if (collider.GetComponent<Health>() == targetedEnemyHealth)
        {
            targetedEnemyHealth = null; 
        }
    }
}
