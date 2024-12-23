using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 1f;
    public int damage = 1;
    private Transform target;
    private ProjectilePool pool;
    private Vector3 lastTargetPosition;

    public void Initialize(Transform target, ProjectilePool pool) {
        this.target = target;
        this.pool = pool;
        lastTargetPosition = target.position;
    }

    void Update() {
        // Get current destination (target if alive, last position if dead)
        Vector3 destination = target != null ? target.position : lastTargetPosition;

        // Move towards destination
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // Return to pool when reached destination
        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            pool.ReturnProjectile(gameObject);
            if (target != null) {
                Health health = target.GetComponent<Health>();
                if (health != null) health.Damage(damage);
            }
        }

        if (target != null) {
            lastTargetPosition = target.position;
        }
    }
}

