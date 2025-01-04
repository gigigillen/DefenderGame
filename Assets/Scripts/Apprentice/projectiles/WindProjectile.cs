using UnityEngine;
using System.Collections;

public class WindProjectile : ProjectileController {

    // unlocked further in the skill tree
    private bool canCreateVortex => SkillManager.IsAbilityUnlocked(ApprenticeType.Wind, "Vortex");
    [SerializeField] private GameObject vfxVortexPrefab;

    private void Awake() {

        projectileType = ApprenticeType.Wind;
    }


    protected override void OnReachTarget() {

        DealDamage();

        if (canCreateVortex && owner != null) {

            if (VaporiseController.CanVaporise(target.gameObject) && VortexController.CanCreateVortex(owner)) {
                Vector3 impactPosition = transform.position;
                impactPosition.y = 0.5f;
                GameObject vortexObject = Instantiate(vfxVortexPrefab, impactPosition, Quaternion.identity);
                VortexEffect vortexEffect = vortexObject.GetComponent<VortexEffect>();
                if (vortexEffect != null) {
                    vortexEffect.StartVortex();
                    VortexController.RecordVortex(owner);
                }
            }
        }

        pool.ReturnProjectile(gameObject);
    }
}
