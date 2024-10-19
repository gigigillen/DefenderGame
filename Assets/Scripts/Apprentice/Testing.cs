using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private UISkillTree uiSkillTree;

    private ApprenticeController selectedApprentice;

    private void Start() {

    }

    public void SelectApprentice(ApprenticeController apprentice) {

        selectedApprentice = apprentice;
        uiSkillTree.SetApprenticeSkills(apprentice.GetApprenticeSkills());
        Debug.Log("Selected Apprentice: " + apprentice.gameObject.name);
        Debug.Log("Skills: " + apprentice.GetApprenticeSkills().GetUnlockedSkills());
    }
}
