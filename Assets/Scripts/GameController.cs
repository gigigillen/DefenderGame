using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    [SerializeField] private UISkillTree uiSkillTree;

    private ApprenticeController selectedApprentice;

    private Camera cam;

    [SerializeField] private LayerMask attackAreaMask;

    private void Start() {

        cam = Camera.main;
        uiSkillTree.SetVisible(false);
    }

    private void Update() {

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~attackAreaMask)) {
                ApprenticeController clickedApprentice = hit.collider.GetComponent<ApprenticeController>();
                if (clickedApprentice != null) {
                    SelectApprentice(clickedApprentice);
                }
                else {
                    DeselectApprentice();
                }
            }
        }
    }

    public void SelectApprentice(ApprenticeController apprentice) {

        if (selectedApprentice != null) {
            DeselectApprentice();
        }

        selectedApprentice = apprentice;
        uiSkillTree.SetApprenticeSkills(apprentice.GetApprenticeSkills());
        uiSkillTree.SetVisible(true);
        apprentice.GetComponent<Renderer>().material.color = Color.yellow;

        Debug.Log("Selected Apprentice: " + apprentice.gameObject.name);
        Debug.Log("Skills: " + apprentice.GetApprenticeSkills().GetUnlockedSkills());
    }

    public void DeselectApprentice() {

        selectedApprentice.GetComponent<Renderer>().material.color = Color.green;
        selectedApprentice = null;
        uiSkillTree.SetVisible(false);
    }
}
