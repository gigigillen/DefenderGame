using UnityEngine;
using System.Collections;

public class EarthProjectile : ProjectileController {

    [Header("Projectile Physics")]
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float arcHeight = 3f;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private ParticleSystem pulseEffect;

    [Header("Impact Effects")]
    [SerializeField] private int damagePerPulse = 3;
    [SerializeField] private float pulseInterval = 1f;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private float aoeRadius = 3f;


    private bool hasPulsingUnlocked = false;
    private int pulseDamageUpgradeLevel = 0;
    private Vector3 initialVelocity;
    private bool hasCrashed;


    private void Awake() {

        projectileType = ApprenticeType.Earth;
        speed = 6f;
        damage = 4;

        crashEffect.Stop();
    }

    public override void Initialize(ApprenticeTypeData typeData, Transform target, ProjectilePool pool, ApprenticeController owner) {
        base.Initialize(typeData, target, pool, owner);
        hasCrashed = false;
        CalculateArcTrajectory();

        // check skill tree status
    }

    private void CalculateArcTrajectory() {

        Vector3 targetPos = target != null ? target.position : lastTargetPosition;

        Vector3 horizontalDiff = new Vector3(
            targetPos.x-transform.position.x,
            0f,
            targetPos.z-transform.position.z
            );
        float horizontalDistance = horizontalDiff.magnitude;
        horizontalDistance = Mathf.Max(horizontalDistance, 1f);

        float timeToTarget = horizontalDistance / speed*2f;
        float adjustedArcHeight = Mathf.Min(arcHeight, horizontalDistance * 0.1f);

        initialVelocity = new Vector3(
            horizontalDiff.x / timeToTarget,
            (adjustedArcHeight+0.5f * gravity * timeToTarget * timeToTarget) / timeToTarget,
            horizontalDiff.z / timeToTarget
        );
    }

    protected override void Update() {
        if (hasCrashed) return;

        float deltaTime = Time.deltaTime;
        initialVelocity.y -= gravity * deltaTime;
        Vector3 moveDirection = initialVelocity * deltaTime;
        float moveDistance = moveDirection.magnitude;
        Ray ray = new Ray(transform.position, moveDirection.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance)) {
            transform.position = hit.point;
            OnReachTarget();
        }
        else {
            transform.position += moveDirection;
        }

        if (target != null) {
            lastTargetPosition = target.position;
        }
    }

    protected override void OnReachTarget() {
        if(!hasCrashed) {
            Vector3 pos = transform.position;
            pos.y = 0.5f;
            transform.position = pos;
            hasCrashed = true;
            StartCoroutine(EarthCrash());
        }
    }

    private IEnumerator EarthCrash() {
        speed = 0f;

        int totalPulseDamage = hasPulsingUnlocked ?
            (damagePerPulse + (pulseDamageUpgradeLevel * 2)) :
            damage;

        int pulseCount = hasPulsingUnlocked ? 3 : 1;


        for (int i = 0; i < pulseCount; i++) {
            crashEffect.Play();
            if (pulseEffect!=null) {
                pulseEffect.Play();
                yield return new WaitForSeconds(pulseInterval);
                pulseEffect.Stop();
            }
            ApplyAoEDamage(totalPulseDamage);
            float waitTime = Mathf.Max(0, pulseInterval - pulseEffect.main.duration);
            yield return new WaitForSeconds(waitTime);
        }
        pool.ReturnProjectile(gameObject);
    }

    private void ApplyAoEDamage(int damage) {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider col in hitColliders) {
            if (col.CompareTag("Enemy") || col.CompareTag("Wizard")) {
                Health health = col.GetComponent<Health>();
                if (health != null) {
                    health.Damage(damage);
                }

                EnemyBehavior enemy = col.GetComponent<EnemyBehavior>();
                if (enemy != null) {
                    enemy.Stun(stunDuration);
                }
            }
        }
    }
}