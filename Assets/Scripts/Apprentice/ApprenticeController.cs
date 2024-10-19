using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeController : MonoBehaviour {

    private ApprenticeSkills apprenticeSkills;

    private void Awake() {

        apprenticeSkills = new ApprenticeSkills();
        gameObject.tag = "Apprentice";
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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
}
