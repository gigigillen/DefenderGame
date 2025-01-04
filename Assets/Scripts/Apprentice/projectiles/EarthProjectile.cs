using UnityEngine;
using System.Collections;

public class EarthProjectile : ProjectileController {

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private ParticleSystem pulseEffect;
    [SerializeField] private float effectsHeightOffset = 0.5f;

    [Header("Impact Effects")]
    [SerializeField] private int damagePerPulse = 3;
    [SerializeField] private float pulseInterval = 1f;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private float aoeRadius = 3f;

    // trajectory settings
    private Vector3 startPos;
    private Vector3 endPos;
    private float arcHeight = 5f;
    private float arcDuration = 1f;
    private float elapsed = 0f;

    private bool canAoePulse => SkillManager.IsAbilityUnlocked(ApprenticeType.Earth, "aoePulse");
    private int pulseDamageUpgradeLevel = 0;
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

        startPos = transform.position;
        endPos = (target != null) ? target.position : lastTargetPosition;
        endPos.y = 1f;
        float horizontalDistance = Vector3.Distance(
            new Vector3(startPos.x, 0f, startPos.z),
            new Vector3(endPos.x, 0f, endPos.z)
        );

        arcDuration = horizontalDistance / speed;
        elapsed = 0f;
    }


    protected override void Update() {

        CalculateTrajectory(Time.captureDeltaTime);
    }

    private void CalculateTrajectory(float deltaTime) {

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / arcDuration);

        Vector3 horizontalPos = Vector3.Lerp(
            new Vector3(startPos.x, 0f, startPos.z),
            new Vector3(endPos.x, 0f, endPos.z),
            t
        );

        float yOffset = 4f * arcHeight * t * (1f - t);
        Vector3 newPos = horizontalPos + Vector3.up * yOffset;
        Vector3 oldPos = transform.position;
        transform.position = newPos;
        Vector3 forward = newPos - oldPos;
        if (forward.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(forward);

        if (Mathf.Approximately(t, 1f)) {
            OnReachTarget();
        }
    }

    protected override void OnReachTarget() {

        if (hasCrashed) return;

        Vector3 pos = transform.position;
        transform.position = pos;
        hasCrashed = true;
        StartCoroutine(EarthCrash());

    }

    private IEnumerator EarthCrash() {
        speed = 0f;

        int totalPulseDamage = canAoePulse ?
            (damagePerPulse + (pulseDamageUpgradeLevel * 2)) :
            damage;

        int pulseCount = canAoePulse ? 3 : 1;

        Vector3 effectPosition = transform.position;
        effectPosition.y += effectsHeightOffset;

        // position effect systems at the offset height
        crashEffect.transform.position = effectPosition;
        if (pulseEffect != null) {
            pulseEffect.transform.position = effectPosition;
        }


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