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

}
