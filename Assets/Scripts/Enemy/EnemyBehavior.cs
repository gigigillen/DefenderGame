using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public GameObject enemy;
    public GameObject stronghold;

    // enemy speed
    public float speed;
    


    void Start()
    {
        

    }

    // enemies uniform attack to the stronghold
    void FixedUpdate()
    {
        // where the enemies should be charging towards
        Vector3 targetPosition = stronghold.transform.position;
        // ensures the enemies attack on a fixed y position
        targetPosition.y = enemy.transform.position.y;

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, speed);

        if (Vector3.Distance(transform.position, stronghold.transform.position) < 2.5)
        {
            HealthBarController.instance.TakeDamage(10f);
            Destroy(gameObject);
        }


    }

    
}
