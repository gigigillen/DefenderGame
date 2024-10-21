using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int health = 100;

    private const int MAX_HEALTH = 100;

    // Start is called before the first frame update
    void Start() { 
    
        
    }

    // Update is called once per frame
    void Update() { 
    
        
    }

    public void Damage(int amount) {

        if (amount < 0) {
            throw new System.ArgumentOutOfRangeException("cannot have negative damage");
        }

        if (health > 0) {
            this.health -= amount;

            if (health <= 0) {
                StartCoroutine(HandleDeath());
            }
        }

    }

    private IEnumerator HandleDeath() {

        yield return new WaitForSeconds(0f);

        Debug.Log("i am dead:(");
        HealthBarController.instance.GainHealth(10f);
        Destroy(gameObject);
    }
}
