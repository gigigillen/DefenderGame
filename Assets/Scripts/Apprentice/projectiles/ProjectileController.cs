using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public float speed = 5f;
    public ApprenticeType projectileType;
    protected Transform target;
    protected ProjectilePool pool;
    protected Vector3 lastTargetPosition;
    protected ApprenticeController owner;

    public virtual void Initialize(Transform target, ProjectilePool pool, ApprenticeController owner) {
        this.target = target;
        this.pool = pool;
        this.owner = owner;
        lastTargetPosition = target.position;
    }

    protected virtual void Update() {
        Vector3 destination = target != null ? target.position : lastTargetPosition;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            OnReachTarget();
        }

        if (target != null) {
            lastTargetPosition = target.position;
        }
    }

    protected virtual void OnReachTarget() {
        pool.ReturnProjectile(gameObject);
    }
}

