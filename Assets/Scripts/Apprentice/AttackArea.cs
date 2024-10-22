using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{

    private int damage = 1;
    private Health targetedEnemyHealth;
    private SpawnApprentice spawnApprentice;

    private void Awake() {

        // find SpawnApprentice script
        // killApprentice() method in SpawnApprentice as it is where
        // apprentice count is dealt with currently
        spawnApprentice = FindAnyObjectByType<SpawnApprentice>();
    }

    private void OnTriggerEnter(Collider collider)
    {

        // if the collider has a Health component and no other enemy is targeted yet
        if (collider.GetComponent<Health>() != null && targetedEnemyHealth == null)
        {
            targetedEnemyHealth = collider.GetComponent<Health>();
            targetedEnemyHealth.Damage(damage);
            Debug.Log("Dealt " + damage + " damage to enemy");

            // check if the enemy is defeated
            int enemyHealth = targetedEnemyHealth.getHealth();
            if (enemyHealth <= 0)
            {
                // destroy the apprentice after defeating the enemy
                Destroy(transform.parent.gameObject);
                spawnApprentice.killApprentice();
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // clear the targeted enemy when it leaves the attack area
        if (collider.GetComponent<Health>() == targetedEnemyHealth)
        {
            targetedEnemyHealth = null; 
        }
    }
}
