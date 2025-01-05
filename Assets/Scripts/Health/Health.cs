using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    private GameController gameController;

    private int enemyMaxHealth = 10;
    private int wizardMaxHealth = 30;

    private int health;
    private int maxHealth;
    public int xpReward = 25; // XP reward for destroying this object
    private XPSystem xpSystem; // Reference to the XP system


    // assigns the health of the game characters at the start
    void Start() {
        gameController = FindFirstObjectByType<GameController>();

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


    private IEnumerator HandleDeath() {

        yield return new WaitForSeconds(0f);
        // Award XP for the kill
        if (xpSystem != null && CompareTag("Enemy")) // Only grant XP for enemies
        {
            xpSystem.AddXP(xpReward);
        }
        else if (CompareTag("Wizard")) {
            gameController.WinGame();
            Debug.Log("The Wicked Wizard is ded");
        }
        Destroy(gameObject);
    }
}