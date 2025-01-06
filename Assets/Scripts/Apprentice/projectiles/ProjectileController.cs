using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    // base stats for projectiles - basic type behaviour
    public ApprenticeType projectileType;
    protected int damage = 2;
    protected float speed;
    protected Transform target;
    protected ProjectilePool pool;
    protected Vector3 lastTargetPosition;
    protected ApprenticeController owner;
    protected ApprenticeTypeData typeData;

    // called when apprentice fires the projectile to setup
    public virtual void Initialize(ApprenticeTypeData typeData, Transform target, ProjectilePool pool, ApprenticeController owner) {
        this.typeData = typeData;
        this.target = target;
        this.pool = pool;
        this.owner = owner;

        speed = typeData.speed;
        projectileType = typeData.type;
        lastTargetPosition = target.position;
    }

    // update projectile positon & rotation each frame
    // basic, fire, water, and wind apprentices movement toward target
    protected virtual void Update() {

        if (target==null) {
            pool.ReturnProjectile(gameObject);
            return;
        }

        // move to target / last know position
        Vector3 destination = target != null ? target.position : lastTargetPosition;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        Vector3 direction = destination - transform.position;
        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            OnReachTarget();
        }

        if (target != null) {
            lastTargetPosition = target.position;
        }
    }

    // called when projectile reaches its target - deal damage & return to pool
    // overriden by projectile types for their unique effects
    protected virtual void OnReachTarget() {

        DealDamage();
        pool.ReturnProjectile(gameObject);
    }

    protected virtual void DealDamage() {
        if (target != null) {
            Health health = target.GetComponent<Health>();
            if (health != null) {
                health.Damage(damage);
            }
        }
    }

    // for burning/wetness interaction
    protected void ApplyVaporizeDamage() {

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null) {
            int vaporizeBonus = 3;
            targetHealth.Damage(vaporizeBonus);
            Debug.Log($"Vaporize dealt {vaporizeBonus} bonus damage!");
        }
    }
}

