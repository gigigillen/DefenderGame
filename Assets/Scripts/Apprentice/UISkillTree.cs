using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour {

    private ApprenticeSkills apprenticeSkills;

    public void BasicSkillClick() {

        if (apprenticeSkills != null) {
            apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Basic);
            Debug.Log("Basic skill unlocked");
        }
        else {
            Debug.Log("No apprentice selected");
        }
    }

    public void MidSkillClick() {

        if (apprenticeSkills != null) {
            apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Mid);
            Debug.Log("Mid skill unlocked");
        }
        else {
            Debug.Log("No apprentice selected");
        }
    }

    public void UltSkillClick() {

        if (apprenticeSkills != null) {
            apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Ultimate);
            Debug.Log("Ultimate skill unlocked");
        }
        else {
            Debug.Log("No apprentice selected");
        }
    }

    public void SetApprenticeSkills(ApprenticeSkills apprenticeSkills) {

        this.apprenticeSkills = apprenticeSkills;
    }
}
