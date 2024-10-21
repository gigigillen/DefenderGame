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

        unlockedSkillTypeList = new List<SkillType>();
    }

    public void UnlockSkill(SkillType skillType) {
        if (!IsSkillUnlocked(skillType)) {
            unlockedSkillTypeList.Add(skillType);
        }
    }

    public bool IsSkillUnlocked(SkillType skillType) {

        return unlockedSkillTypeList.Contains(skillType);
    }

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
