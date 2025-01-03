using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private int enemyMaxHealth = 20;
    [SerializeField] private int wizardMaxHealth = 30;

    private int health;
    private int maxHealth;
    public int xpReward = 25; // XP reward for destroying this object
    private XPSystem xpSystem; // Reference to the XP system


    // assigns the health of the game characters at the start
    void Start() {

        // Locate the XPSystem in the scene
        xpSystem = XPSystem.FindFirstObjectByType<XPSystem>();
        if (CompareTag("Enemy"))
        {
            maxHealth = enemyMaxHealth;
        }
        // if it is a special enemy (ie wizard) it gets assigned more health
        else if (CompareTag("Wizard"))
        {
            maxHealth = wizardMaxHealth;
        }

        health = maxHealth;
    }


    public int GetMaxHealth() {
        return maxHealth;
    }


    // health getter method
    public int getHealth() {
        return health;
    }


    // deals with damage taken
    public void Damage(int amount) {
        //throws error if it is less than 0
        if (amount < 0) {
            throw new System.ArgumentOutOfRangeException("cannot have negative damage");
        }

        // works out damage taken
        if (health > 0) {
            this.health -= amount;
            // if 0 or less kills off game object
            if (health <= 0) {
                StartCoroutine(HandleDeath());
            }
        }

    }


    // gives the stronghold 10% more health and destroys game object
    // the IEnumerator return is a future implementation
    private IEnumerator HandleDeath() {

        yield return new WaitForSeconds(0f);

        Debug.Log("i am dead:(");
        HealthBarController.instance.GainHealth(10f);
        // Award XP for the kill
        if (xpSystem != null && CompareTag("Enemy")) // Only grant XP for enemies
        {
            xpSystem.AddXP(xpReward);
        }
        Destroy(gameObject);
    }
}