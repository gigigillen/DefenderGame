using UnityEngine;
using System.Collections;

public class WindProjectile : ProjectileController {

    // unlocked further in the skill tree
    private bool canCreateVortex = true;
    [SerializeField] private GameObject vfxVortexPrefab;

    private void Awake() {

        projectileType = ApprenticeType.Wind;
    }

    // call this when vortex skill is unlocked
    public void EnableVortex() {

        canCreateVortex = true;
    }

    protected override void OnReachTarget() {

        DealDamage();

        if (canCreateVortex && owner != null) {

            if (VaporiseController.CanVaporise(target.gameObject) && VortexController.CanCreateVortex(owner)) {
                Vector3 impactPosition = transform.position;
                impactPosition.y = 0.1f;
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
