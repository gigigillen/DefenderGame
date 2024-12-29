using UnityEngine;
using System.Collections;

public class FireProjectile : ProjectileController {

    //public GameObject crashEffectPrefab; // Optional VFX

    private void Awake() {
        projectileType = ApprenticeType.Fire;
        speed = 3f;
    }
}
