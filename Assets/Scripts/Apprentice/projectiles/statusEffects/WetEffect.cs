using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetEffect : StatusEffect { 

    private EnemyBehavior enemyBehavior;
    private float originalSpeed;
    private float speedReduction = 0.5f;

    private GameObject vfxPrefab;

    private void Awake() {
        enemyBehavior = GetComponent<EnemyBehavior>();

        if (enemyBehavior != null) {
            originalSpeed = enemyBehavior.speed;
        }

    }

    public void StartWetness(float duration, GameObject wetVfxPrefab) {

        if (activeVfx == null && wetVfxPrefab != null) {
            vfxPrefab = wetVfxPrefab;
            activeVfx = Instantiate(vfxPrefab, transform);
            activeVfx.transform.localPosition = new Vector3(0.5f, 0.5f, 0.1f);
        }

        if (enemyBehavior != null) {
            enemyBehavior.speed = originalSpeed * speedReduction;
        }

        RefreshDuration(duration);
    }

    public override void RemoveEffect() {


        if (enemyBehavior != null) {
            enemyBehavior.speed = originalSpeed;
        }

        base.RemoveEffect();
    }
}
