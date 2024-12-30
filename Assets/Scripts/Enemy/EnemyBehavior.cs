using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] private Transform canvasTransform;

    public GameObject enemy;
    public GameObject stronghold;

    private bool isStunned = false;
    private float stunTimeLeft = 0f;

    // enemy speed
    public float speed;


    void Start()
    {

    }

    public void Stun(float duration) {
        stunTimeLeft = Mathf.Max(stunTimeLeft, duration);
        isStunned = false;
    }

    // enemies uniform attack to the stronghold
    void FixedUpdate()
    {

        if (isStunned) {
            stunTimeLeft -= Time.deltaTime;
            if (stunTimeLeft <= 0) {
                isStunned = false;
            }
            return;
        }

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


    private void LateUpdate() {

        canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
