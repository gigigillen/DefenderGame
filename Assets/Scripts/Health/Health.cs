using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    private GameController gameController;

    private int enemyMaxHealth = 30;
    private int wizardMaxHealth = 80;

    private int health;
    private int maxHealth;
    // XP reward for destroying this object
    public int xpReward = 25;
    // Reference to the XP system
    private XPSystem xpSystem; 


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
        // Award XP for the kill, only grant XP for enemies
        if (xpSystem != null && CompareTag("Enemy")) 
        {
            xpSystem.AddXP(xpReward);
        }
        // If wizard defeated carries out win game method
        else if (CompareTag("Wizard")) {
            gameController.WinGame();
        }
        Destroy(gameObject);
    }
}