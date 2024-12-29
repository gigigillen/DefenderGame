using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeSkills { 

    // in prototype, only the Basic skill works. Mid and Ultimate are placeholders
    public enum SkillType {
        Basic,
        Mid,
        Ultimate,
    }

    private List<SkillType> unlockedSkillTypeList;

    public ApprenticeSkills() {

        // initialise list of unlocked skills in constructor
        unlockedSkillTypeList = new List<SkillType>();
        unlockedSkillTypeList.Add(SkillType.Basic);
    }

    public void UnlockSkill(SkillType skillType) {
        // only add the skill if not unlocked already
        if (!IsSkillUnlocked(skillType)) {
            unlockedSkillTypeList.Add(skillType);
        }
        else {
        }
    }

    public bool IsSkillUnlocked(SkillType skillType) {

        return unlockedSkillTypeList.Contains(skillType);
    }

    // returns a string of unclocked skills, currently used for debugging
    public string GetUnlockedSkills() {
        if (unlockedSkillTypeList.Count == 0) {
            return "No skills unlocked";
        }

        string skillList = "Unlocked Skills: ";
        foreach (SkillType skill in unlockedSkillTypeList) {
            skillList += skill.ToString() + ", ";
        }

        skillList = skillList.TrimEnd(',', ' ');
        return skillList;
    }

}
