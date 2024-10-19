using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeSkills { 

    public enum SkillType {
        Basic,
    }

    private List<SkillType> unlockedSkillTypeList;

    public ApprenticeSkills() {

        unlockedSkillTypeList = new List<SkillType>();
    }

    public void UnlockSkill(SkillType skillType) {

        unlockedSkillTypeList.Add(skillType);
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
