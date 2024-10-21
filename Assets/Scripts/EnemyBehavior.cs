using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public GameObject enemy;
    public GameObject stronghold;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Reset the Y position of the stronghold
        Vector3 targetPosition = stronghold.transform.position;
        targetPosition.y = enemy.transform.position.y; // Set Y to the enemy's Y position or a fixed value

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, speed);

        if (Vector3.Distance(transform.position, stronghold.transform.position) < 2.5)
        {
            HealthBarController.instance.TakeDamage(10f);
            Destroy(gameObject);
        }


    }
}
