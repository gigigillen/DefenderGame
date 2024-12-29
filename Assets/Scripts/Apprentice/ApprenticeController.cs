using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeController : MonoBehaviour {

    [SerializeField] private ApprenticeTypeData typeData;
    [SerializeField] private TargetingStrategy currentStrategy = TargetingStrategy.ClosestToStronghold;
    [SerializeField] private GameObject stronghold;

    public ApprenticeType apprenticeType;
    private float currentCooldown;

    private ApprenticeSkills apprenticeSkills;
    private ApprenticeAttack apprenticeAttack;
    private ProjectilePool projectilePool;

    private GameObject enemyToTarget;
    private float nearestDist;

    private bool hasInitialShot = false;


    private void Awake() {
        // apprentice skills initialised, and attack component retrieved
        apprenticeSkills = new ApprenticeSkills();
        if (!typeData.isStatic) {
            apprenticeAttack = GetComponent<ApprenticeAttack>();
        }
        projectilePool = ProjectilePool.FindAnyObjectByType<ProjectilePool>();
        currentCooldown = 0f;
        gameObject.tag = "Apprentice";
    }


    void Update() {

        if (apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Basic)) {
            if (currentCooldown>0) {
                currentCooldown -= Time.deltaTime;
            }
            FindEnemyToTarget();
            if (enemyToTarget != null) {
                HandleAttack();
            }
        }
    }


    private void HandleAttack() {
        if(!typeData.isStatic) {
            MeleeAttack();
        }
        else if (nearestDist<=typeData.attackRange) {

            Vector3 directionToEnemy = enemyToTarget.transform.position - transform.position;
            directionToEnemy.y = 0f;
            transform.rotation = Quaternion.LookRotation(directionToEnemy);

            if(!hasInitialShot || currentCooldown<=0) {
                RangedAttack();
                hasInitialShot = true;
                currentCooldown = typeData.cooldown;
            }
        }
    }

    private void MeleeAttack() {
        Vector3 targetPos = enemyToTarget.transform.position;
        // keep y position consistent
        targetPos.y = transform.position.y;

        // only move towards the enemy if not within attack range
        if (nearestDist > 1.0f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, typeData.speed * Time.deltaTime);
        }
        else {
            // launch attack when in range
            apprenticeAttack.Attack();
        }
}

    private void RangedAttack() {

        GameObject projectile = projectilePool.GetProjectile(apprenticeType);
        projectile.transform.position = transform.position + transform.forward;
        projectile.GetComponent<ProjectileController>().Initialize(enemyToTarget.transform, projectilePool, this);
    }


    public ApprenticeSkills GetApprenticeSkills() {

        return apprenticeSkills;
    }

    public bool CanUseBasicSkill() {

        return apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Basic);
    }

    // CanUseMidSkill and CanUseUltimateSkill are placeholders for later implementation
    public bool CanUseMidSkill() {

        return apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Mid);
    }

    public bool CanUseUltimateSkill() {

        return apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Ultimate);
    }

    //calculate a score for each potential enemy to target
    //lowest score = best target
    private void FindEnemyToTarget() {

        // enemies can have either tag "Enemy" or "Wizard", both are searched for
        string[] tags = { "Enemy", "Wizard" };
        float bestScore = float.MaxValue;
        GameObject bestTarget = null;
        nearestDist = float.MaxValue;

        foreach (string tag in tags) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies) {
                if (!enemy.activeSelf) continue;

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= typeData.attackRange) {
                    Health enemyHealth = enemy.GetComponent<Health>();
                    if (enemyHealth == null || enemyHealth.getHealth() <= 0) continue;

                    float score = EvaluateTarget(enemy, enemyHealth);

                    if (score < bestScore) {
                        if (bestTarget != null) {
                            bestTarget.GetComponent<Renderer>().material.color = Color.red;
                        }
                        bestScore = score;
                        bestTarget = enemy;
                        bestTarget.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                }
            }
        }

        enemyToTarget = bestTarget;
        if (bestTarget!=null) {
            nearestDist = Vector3.Distance(transform.position, bestTarget.transform.position);
        }
    }


    private float EvaluateTarget(GameObject enemy, Health enemyHealth) {
        float distanceToTower = Vector3.Distance(transform.position, enemy.transform.position);
        float distanceToStronghold = Vector3.Distance(stronghold.transform.position, enemy.transform.position);

        if (distanceToTower > typeData.attackRange) {
            return float.MaxValue;
        }

        switch (currentStrategy) {
            //basic targeting available by default
            case TargetingStrategy.ClosestToTower:
                return distanceToTower;

            //defensive instincts - low tier unlocked skill
            case TargetingStrategy.ClosestToStronghold:
                return distanceToStronghold;

            //threat awareness - mid tier unlocked skill
            case TargetingStrategy.MostDangerous:
                float dangerMultiplier = enemy.CompareTag("Wizard") ? 0.5f : 1.0f;
                return distanceToTower * dangerMultiplier;

            //focused fire - high tier unlocked skill
            case TargetingStrategy.LowestHealth:
                float healthScore = enemyHealth.getHealth() * 10f;
                return healthScore + (distanceToTower * 0.5f);

            default:
                return float.MaxValue;
        }
    }

    public void SetTargetingStrategy(TargetingStrategy strategy) {
        currentStrategy = strategy;

        //clear current target
        if (enemyToTarget!=null) {
            enemyToTarget.GetComponent<Renderer>().material.color = Color.red;
            enemyToTarget = null;
        }
    }
}
