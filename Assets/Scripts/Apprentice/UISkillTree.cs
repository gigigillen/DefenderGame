using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour
{
    private XPSystem xpSystem; // Reference to the XP system

    private void Start()
    {
        // Locate the XPSystem in the scene
        xpSystem = XPSystem.FindFirstObjectByType<XPSystem>();
    }

    //// Method called when B (basic) skill button pressed in the UI skill tree
    //public void BasicSkillClick()
    //{
    //    // Unlock basic skill for selected apprentice (no skill point required)
    //    if (apprenticeSkills != null)
    //    {
    //        apprenticeSkills.UnlockSkill(ApprenticeSkills.SkillType.Basic);
    //        Debug.Log("Basic skill unlocked");
    //    }
    //    else
    //    {
    //        Debug.Log("No apprentice selected");
    //    }
    //}

    //// Method called when Earth skill button is pressed
    //public void EarthSkillClick()
    //{
    //    UnlockSkill(ApprenticeSkills.SkillType.Earth, "Earth skill unlocked");
    //}

    //// Method called when Fire skill button is pressed
    //public void FireSkillClick()
    //{
    //    UnlockSkill(ApprenticeSkills.SkillType.Fire, "Fire skill unlocked");
    //}

    //// Method called when Water skill button is pressed
    //public void WaterSkillClick()
    //{
    //    UnlockSkill(ApprenticeSkills.SkillType.Water, "Water skill unlocked");
    //}

    //// Method called when Air skill button is pressed
    //public void WindSkillClick()
    //{
    //    UnlockSkill(ApprenticeSkills.SkillType.Wind, "Wind skill unlocked");
    //}

    //// Generic method to unlock a skill with a skill point
    //private void UnlockSkill(ApprenticeSkills.SkillType skillType, string successMessage)
    //{
    //    if (xpSystem != null && xpSystem.SpendSkillPoint())
    //    {
    //        if (apprenticeSkills != null)
    //        {
    //            apprenticeSkills.UnlockSkill(skillType);
    //            Debug.Log(successMessage);
    //        }
    //        else
    //        {
    //            Debug.Log("No apprentice selected");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Not enough skill points to unlock this skill");
    //    }
    //}


    // Show/hide the UI skill tree
    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}

