using UnityEngine;
using System.Collections;

public class WindProjectile : ProjectileController {

    //public GameObject crashEffectPrefab; // Optional VFX

    private void Awake() {
        projectileType = ApprenticeType.Wind;
    }
}
