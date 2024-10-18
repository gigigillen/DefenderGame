using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public GameObject enemy;
    public GameObject stronghold;
    public float speed;

    //public float detectionRadius = 0; // Adjust this for how close the enemy needs to be to the stronghold

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
            ScoreManager.instance.SubtractPoint(); // Decrement a point
            Destroy(gameObject); // Destroy this enemy object
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Apprentice"))
        {
            ScoreManager.instance.AddPoint();
            Destroy(gameObject);
        }
    }
}
