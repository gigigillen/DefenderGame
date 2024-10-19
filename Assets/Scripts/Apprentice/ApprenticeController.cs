using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ApprenticeController : MonoBehaviour {

    private ApprenticeSkills apprenticeSkills;

    private Camera cam;

    private void Awake() {

        apprenticeSkills = new ApprenticeSkills();
    }

    // Start is called before the first frame update
    void Start() {

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnSelect() {

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == this.gameObject) {
                    SelectApprentice();
                }
            }
        }
    }

    private void SelectApprentice() {

        Testing testing = FindAnyObjectByType<Testing>();
        testing.SelectApprentice(this);
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
