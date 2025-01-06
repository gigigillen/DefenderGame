using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeController : MonoBehaviour
{
    // configuration data for this apprentice type
    [SerializeField] private ApprenticeTypeData typeData;
    // controls how this apprentice chooses targets
    [SerializeField] private TargetingStrategy currentStrategy = TargetingStrategy.ClosestToStronghold;
    [SerializeField] private GameObject stronghold;

    public ApprenticeType apprenticeType; // Type of apprentice (Basic, Water, Wind, Earth, Fire)
    private float currentCooldown;

    private ProjectilePool projectilePool;

    private GameObject enemyToTarget;
    private float nearestDist;

    private bool hasInitialShot = false;

    private void Awake()
    {

        projectilePool = ProjectilePool.FindAnyObjectByType<ProjectilePool>();
        currentCooldown = 0f;
        gameObject.tag = "Apprentice";
    }

    // calculate current cooldown and find enemies to attack within range
    void Update()
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

    // rotate to face the enemy within range and attack when off cooldown
    private void HandleAttack()
    {

        if (nearestDist <= typeData.attackRange)
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

    // get a new projectile from projectile pool and initialise with target information
    private void RangedAttack()
    {
        GameObject projectile = projectilePool.GetProjectile(apprenticeType);
        projectile.transform.position = transform.position + transform.forward;
        projectile.GetComponent<ProjectileController>().Initialize(typeData, enemyToTarget.transform, projectilePool, this);
    }

    // scan for enemies/wizard within attack range and give score based on targetting strategy
    // update selected target if a 'better' one is found
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

    // calculate score for targetting, lower score = higher priority targeting
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
