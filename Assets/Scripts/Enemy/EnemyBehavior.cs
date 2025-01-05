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
    private GameController gameController;

    // enemy speed
    public float speed;

    private void Start() {

        gameController = FindFirstObjectByType<GameController>();
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

        if (Vector3.Distance(transform.position, stronghold.transform.position) < 5)
        {
            if (enemy.tag=="Wizard") {
                gameController.GameOver();
            }
            else {
                HealthBarController.instance.TakeDamage(10f);
                Destroy(gameObject);
            }
        }
    }


    private void LateUpdate() {

        canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
