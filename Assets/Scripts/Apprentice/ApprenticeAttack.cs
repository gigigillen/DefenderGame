using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeAttack : MonoBehaviour { 

    private GameObject attackArea = default;
    private ApprenticeSkills apprenticeSkills;

    private bool attacking = false;

    // for prototype, attack area of apprentice is active for 0.05s
    private float timeToAttack = 0.05f;
    private float timer = 0f;

    void Start() {

        // get attack area GameObject, which is a child object of the apprentice
        attackArea = transform.GetChild(0).gameObject;

        // retrieve apprentice skills
        apprenticeSkills = GetComponentInParent<ApprenticeController>().GetApprenticeSkills();
    }

    // handles attack timing
    // attack area deactivated after timeToAttack seconds
    void FixedUpdate() {

        if (attacking) {
            timer += Time.deltaTime;

            if (timer >= timeToAttack) {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    // an attack is initiated if the basic skill is unlocked
    public void Attack() {

        if (apprenticeSkills != null && apprenticeSkills.IsSkillUnlocked(ApprenticeSkills.SkillType.Basic))
        {
            attacking = true;
            attackArea.SetActive(attacking);
        }
        else
        {
            Debug.Log("basic skill not unlocked - no attack");
 
        }
    }

}
