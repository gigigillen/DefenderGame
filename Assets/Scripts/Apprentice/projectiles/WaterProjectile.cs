using UnityEngine;
using System.Collections;

public class WaterProjectile : ProjectileController {

    [Header("Hit Effects")]
    private int damage = 1;
    //public GameObject crashEffectPrefab; // Optional VFX

    private void Awake() {
        projectileType = ApprenticeType.Water;
        speed = 3f;
    }

    protected override void OnReachTarget() {
        if (target != null) {
            Health health = target.GetComponent<Health>();
            if (health != null) {
                health.Damage(damage);
            }
        }
        pool.ReturnProjectile(gameObject);
    }
}
