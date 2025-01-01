using UnityEngine;
using System.Collections;

public class WaterProjectile : ProjectileController {

    private bool canApplyWetness = true;
    [SerializeField] private float wetnessDuration = 5f;
    [SerializeField] private GameObject vfxWetPrefab;


    private void Awake() {
        projectileType = ApprenticeType.Water;
    }

    protected override void OnReachTarget() {

        BurningEffect burningEffect = target.GetComponent<BurningEffect>();

        if (burningEffect != null) {
            if (VaporiseController.CanVaporise(target.gameObject)) {
                DealDamage();
                base.OnReachTarget();
                Debug.Log("vaporise triggered!");
                burningEffect.RemoveEffect();
                VaporiseController.RecordVaporise(target.gameObject);
            }
        }
        else {
            base.OnReachTarget();

            if (canApplyWetness && target != null) {

                WetEffect wetnessEffect = target.GetComponent<WetEffect>();

                if (wetnessEffect != null) {
                    wetnessEffect.StartWetness(wetnessDuration, vfxWetPrefab);
                }
                else {
                    WetEffect newWetnessEffect = target.gameObject.AddComponent<WetEffect>();
                    newWetnessEffect.StartWetness(wetnessDuration, vfxWetPrefab);
                }
            }
        }
    }
}
