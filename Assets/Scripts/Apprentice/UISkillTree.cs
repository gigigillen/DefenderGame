using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour {

    private ApprenticeSkills apprenticeSkills;

    // method called when B (basic) skill button pressed in the UI skill tree
    public void BasicSkillClick() {

        // unlock basic skill for selected apprentice
        if (apprenticeSkills != null) {
            apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Basic);
            Debug.Log("Basic skill unlocked");
        }
        else {
            Debug.Log("No apprentice selected");
        }
    }

    // mid and ultimate skills are placeholders in the prototype for further implementation
    // method called when M (mid) skill button pressed in the UI skill tree
    public void MidSkillClick() {

        // unlock mid skill for selected apprentice
        if (apprenticeSkills != null) {
            apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Mid);
            Debug.Log("Mid skill unlocked");
        }
        else {
            Debug.Log("No apprentice selected");
        }
    }

    // method called when U (ultimate) skill button pressed in the UI skill tree
    public void UltSkillClick() {

        // unlock ultimate skill for selected apprentice
        if (apprenticeSkills != null) {
            apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Ultimate);
            Debug.Log("Ultimate skill unlocked");
        }
        else {
            Debug.Log("No apprentice selected");
        }
    }

    // set apprentice skills to be modified using the UI skill tree
    public void SetApprenticeSkills(ApprenticeSkills apprenticeSkills) {

        this.apprenticeSkills = apprenticeSkills;
    }

    // show/hide the UI skill tree
    public void SetVisible(bool isVisible) {
        gameObject.SetActive(isVisible);
    }

}
