using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeController : MonoBehaviour {

    public ApprenticeType apprenticeType;

    private ApprenticeSkills apprenticeSkills;
    private ApprenticeAttack apprenticeAttack;

    private GameObject nearestEnemy;
    private float nearestDist;

    public float speed;

    private void Awake() {

        // apprentice skills initialised, and attack component retrieved
        apprenticeSkills = new ApprenticeSkills();
        apprenticeAttack = GetComponent<ApprenticeAttack>();
        gameObject.tag = "Apprentice";
    }

    void Start() {

    }

    void Update() {

        // if basic skill unlocked, find and move towards nearest enemy
        if (apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Basic)) {
            FindNearestEnemy();
            if (nearestEnemy != null) {
                MoveTowardsEnemy();
            }
        }
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

    private void FindNearestEnemy() {

        // enemies can have either tag "Enemy" or "Wizard", both are searched for
        string[] tags = { "Enemy", "Wizard" };
        nearestDist = float.MaxValue;

        foreach (string tag in tags) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {
                if (enemy.activeSelf)
                {
                    float dist = Vector3.Distance(transform.position, enemy.transform.position);

                    if (dist < nearestDist)
                    {
                        if (nearestEnemy != null)
                        {
                            // reset colour of previous nearest enemy back to red
                            nearestEnemy.GetComponent<Renderer>().material.color = Color.red;
                        }

                        // set new nearest enemy, change its colour to magenta
                        nearestDist = dist;
                        nearestEnemy = enemy;
                        nearestEnemy.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                }
            }
        }
    }

    private void MoveTowardsEnemy() {

        Vector3 targetPos = nearestEnemy.transform.position;
        // keep y position consistent
        targetPos.y = transform.position.y;

        // only move towards the enemy if not within attack range
        if (nearestDist > 1.0f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else {
            // launch attack when in range
            apprenticeAttack.Attack();
        }
    }
}
