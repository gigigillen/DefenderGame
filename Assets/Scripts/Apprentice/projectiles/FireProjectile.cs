using UnityEngine;
using System.Collections;

public class FireProjectile : ProjectileController {

    // unlocked further in the skill tree
    private bool canApplyBurning = true;
    [SerializeField] private float burnDuration = 3f;
    [SerializeField] private GameObject vfxBurnPrefab;

    private void Awake() {

        projectileType = ApprenticeType.Fire;
    }

    // call this when burning skill is unlocked
    public void EnableBurning() {

        canApplyBurning = true;
    }

    protected override void OnReachTarget() {
        base.OnReachTarget();

        if (canApplyBurning && target != null) {

            BurningEffect burningEffect = target.GetComponent<BurningEffect>();

            if (burningEffect != null) {
                burningEffect.StartBurning(burnDuration, vfxBurnPrefab);
            }
            else {
                BurningEffect newBurningEffect = target.gameObject.AddComponent<BurningEffect>();
                newBurningEffect.StartBurning(burnDuration, vfxBurnPrefab);
            }
        }
    }
}
