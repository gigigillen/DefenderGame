using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeController : MonoBehaviour {

    private ApprenticeSkills apprenticeSkills;
    private ApprenticeAttack apprenticeAttack;
    private SpawnApprentice spawnApprentice;

    private GameObject nearestEnemy;
    private float nearestDist;

    public float speed;

    private void Awake() {

        apprenticeSkills = new ApprenticeSkills();
        apprenticeAttack = GetComponent<ApprenticeAttack>();
        spawnApprentice = FindAnyObjectByType<SpawnApprentice>();
        gameObject.tag = "Apprentice";
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Basic))
        {
            FindNearestEnemy();

            if (nearestEnemy != null)
            {
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

    public bool CanUseMidSkill() {

        return apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Mid);
    }

    public bool CanUseUltimateSkill() {

        return apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Ultimate);
    }

    private void FindNearestEnemy() {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        nearestDist = float.MaxValue;

        foreach (GameObject enemy in enemies) {
            if (enemy.activeSelf) {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (dist < nearestDist) {
                    if (nearestEnemy != null) {
                        nearestEnemy.GetComponent<Renderer>().material.color = Color.red;
                    }

                    nearestDist = dist;
                    nearestEnemy = enemy;
                    nearestEnemy.GetComponent<Renderer>().material.color = Color.magenta;
                }
            }
        }
    }

    private void MoveTowardsEnemy() {

        Vector3 targetPos = nearestEnemy.transform.position;
        targetPos.y = transform.position.y; // ensure y value stays consistent

        if (nearestDist > 1.0f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else {
            apprenticeAttack.Attack();
 //           Destroy(gameObject);
        }
    }
}
