using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private UISkillTree uiSkillTree;

    private ApprenticeController selectedApprentice;

    private void Start() {

        if (uiSkillTree == null) {
            Debug.LogError("UI Skill tree is not assigned!");
        }
    }

    public void SelectApprentice(ApprenticeController apprentice) {

        selectedApprentice = apprentice;
        uiSkillTree.SetApprenticeSkills(apprentice.GetApprenticeSkills());
    }
}
