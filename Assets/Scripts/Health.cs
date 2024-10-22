using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    private int health;

    private const int MAX_HEALTH = 100;

    // assigns the health of the game characters at the start
    void Start() {
        if (CompareTag("Enemy"))
        {
            health = 1;
        }
        // if it is a special enemy (ie wizard) it gets assigned more health
        else if (CompareTag("Wizard"))
        {
            health = 10;
        }
    }

    // Update is called once per frame
    void Update() { 
    
        
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
        Destroy(gameObject);
    }
}
