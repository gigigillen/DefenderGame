using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : StatusEffect {

    private Health targetHealth;
    private int dmgPerTick = 1;
    private GameObject vfxPrefab;

    private void Awake() {
        tickRate = 1f;
        targetHealth = GetComponent<Health>();
    }

    public void StartBurning(float duration, GameObject burnVfxPrefab) {

        if (activeVfx == null && burnVfxPrefab != null) {
            vfxPrefab = burnVfxPrefab;
            activeVfx = Instantiate(vfxPrefab, transform);
            activeVfx.transform.localPosition = new Vector3(0.5f, 0.2f, 0.1f);
        }

        RefreshDuration(duration);
    }

    protected override void ApplyEffectTick() {
        if (targetHealth != null) {
            targetHealth.Damage(dmgPerTick);
        }
    }
}
