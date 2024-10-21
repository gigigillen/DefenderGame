using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeAttack : MonoBehaviour { 

    private GameObject attackArea = default;
    private ApprenticeSkills apprenticeSkills;

    private bool attacking = false;

    // for prototype, 1 dmg kills an enemy so time interval between attacks not required
    // for further implementation later
    private float timeToAttack = 0.01f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start() {

        attackArea = transform.GetChild(0).gameObject;

        // get the unlocked apprentice skills of the specific apprentice
        apprenticeSkills = GetComponentInParent<ApprenticeController>().GetApprenticeSkills();
    }

    // Update is called once per frame
    // handle attack timing - timer counts to duration of timeToAttack then resets attackArea
    void Update() {

        if (attacking) {
            timer += Time.deltaTime;

            if (timer >= timeToAttack) {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    public void Attack() {

        // for prototype, apprentice can only be trained with one skill - Basic attack
        // until this skill is trained, the apprentice will not attack
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
