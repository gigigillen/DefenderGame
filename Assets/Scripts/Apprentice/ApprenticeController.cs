using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeController : MonoBehaviour
{
    [SerializeField] private ApprenticeTypeData typeData;
    [SerializeField] private TargetingStrategy currentStrategy = TargetingStrategy.ClosestToStronghold;
    [SerializeField] private GameObject stronghold;

    public ApprenticeType apprenticeType; // Type of apprentice (Basic, Water, Wind, Fire)
    private float currentCooldown;

    private ApprenticeSkills apprenticeSkills;
    private ApprenticeAttack apprenticeAttack;
    private ProjectilePool projectilePool;

    private GameObject enemyToTarget;
    private float nearestDist;

    private bool hasInitialShot = false;

    private void Awake()
    {
        // Initialize apprentice skills and components
        apprenticeSkills = new ApprenticeSkills();
        if (!typeData.isStatic)
        {
            apprenticeAttack = GetComponent<ApprenticeAttack>();
        }
        projectilePool = ProjectilePool.FindAnyObjectByType<ProjectilePool>();
        currentCooldown = 0f;
        gameObject.tag = "Apprentice";
    }

    void Update()
    {
        // Check if the apprentice has unlocked the skill corresponding to their type
        if (apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Basic))
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
            FindEnemyToTarget();
            if (enemyToTarget != null)
            {
                HandleAttack();
            }
        }
    }

    private void HandleAttack()
    {
        if (!typeData.isStatic)
        {
            MeleeAttack();
        }
        else if (nearestDist <= typeData.attackRange)
        {
            Vector3 directionToEnemy = enemyToTarget.transform.position - transform.position;
            directionToEnemy.y = 0f;
            transform.rotation = Quaternion.LookRotation(directionToEnemy);

            if (!hasInitialShot || currentCooldown <= 0)
            {
                RangedAttack();
                hasInitialShot = true;
                currentCooldown = typeData.cooldown;
            }
        }
    }

    private void MeleeAttack()
    {
        Vector3 targetPos = enemyToTarget.transform.position;
        targetPos.y = transform.position.y;

        if (nearestDist > 1.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, typeData.speed * Time.deltaTime);
        }
        else
        {
            apprenticeAttack.Attack();
        }
    }

    private void RangedAttack()
    {
        GameObject projectile = projectilePool.GetProjectile(apprenticeType);
        projectile.transform.position = transform.position + transform.forward;
        projectile.GetComponent<ProjectileController>().Initialize(typeData, enemyToTarget.transform, projectilePool, this);
    }

    public ApprenticeSkills GetApprenticeSkills()
    {
        return apprenticeSkills;
    }

    // Determine the skill type corresponding to the apprentice type
    private ApprenticeSkills.SkillType GetCorrespondingSkillType()
    {
        switch (apprenticeType)
        {
            case ApprenticeType.Basic: return ApprenticeSkills.SkillType.Basic;
            case ApprenticeType.Water: return ApprenticeSkills.SkillType.Water;
            case ApprenticeType.Wind: return ApprenticeSkills.SkillType.Wind;
            case ApprenticeType.Fire: return ApprenticeSkills.SkillType.Fire;
            case ApprenticeType.Earth: return ApprenticeSkills.SkillType.Earth;
            default: return ApprenticeSkills.SkillType.Basic;
        }
    }

    private void FindEnemyToTarget()
    {
        string[] tags = { "Enemy", "Wizard" };
        float bestScore = float.MaxValue;
        GameObject bestTarget = null;
        nearestDist = float.MaxValue;

        foreach (string tag in tags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {
                if (!enemy.activeSelf) continue;

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= typeData.attackRange)
                {
                    Health enemyHealth = enemy.GetComponent<Health>();
                    if (enemyHealth == null || enemyHealth.getHealth() <= 0) continue;

                    float score = EvaluateTarget(enemy, enemyHealth);

                    if (score < bestScore)
                    {
                        if (bestTarget != null)
                        {
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
        if (bestTarget != null)
        {
            nearestDist = Vector3.Distance(transform.position, bestTarget.transform.position);
        }
    }

    private float EvaluateTarget(GameObject enemy, Health enemyHealth)
    {
        float distanceToTower = Vector3.Distance(transform.position, enemy.transform.position);
        float distanceToStronghold = Vector3.Distance(stronghold.transform.position, enemy.transform.position);

        if (distanceToTower > typeData.attackRange)
        {
            return float.MaxValue;
        }

        switch (currentStrategy)
        {
            case TargetingStrategy.ClosestToTower:
                return distanceToTower;

            case TargetingStrategy.ClosestToStronghold:
                return distanceToStronghold;

            case TargetingStrategy.MostDangerous:
                float dangerMultiplier = enemy.CompareTag("Wizard") ? 0.5f : 1.0f;
                return distanceToTower * dangerMultiplier;

            case TargetingStrategy.LowestHealth:
                float healthScore = enemyHealth.getHealth() * 10f;
                return healthScore + (distanceToTower * 0.5f);

            default:
                return float.MaxValue;
        }
    }

    public void SetTargetingStrategy(TargetingStrategy strategy)
    {
        currentStrategy = strategy;

        if (enemyToTarget != null)
        {
            enemyToTarget.GetComponent<Renderer>().material.color = Color.red;
            enemyToTarget = null;
        }
    }
}
