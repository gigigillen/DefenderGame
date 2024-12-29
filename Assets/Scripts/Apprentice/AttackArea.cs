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

                ApprenticeController apprentice = transform.parent.GetComponent<ApprenticeController>();
                spawnApprentice.RemoveApprentice(apprentice);
                // destroy apprentice after removing it from tracking
                Destroy(transform.parent.gameObject);
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
