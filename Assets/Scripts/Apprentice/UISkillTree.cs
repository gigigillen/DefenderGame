using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour { 

    [SerializeField] private GameObject skillTreeContents;
    [SerializeField] private GameObject placementTipPanel;

    private bool isPlacementActive = false;

    private XPSystem xpSystem; // Reference to the XP system


    private void Start() { 
    
        // Locate the XPSystem in the scene
        xpSystem = XPSystem.FindFirstObjectByType<XPSystem>();
        placementTipPanel.SetActive(false);
        skillTreeContents.SetActive(true);
    }


    public void OnPlacementStart() {

        Debug.Log("trying to show placement tip");
        isPlacementActive = true;
        skillTreeContents.SetActive(false);
        placementTipPanel.SetActive(true);
    }


    public void OnPlacementEnd() {

        Debug.Log("trying to hide placement tip");
        isPlacementActive = false;

        // Switch back to normal skill tree view
        skillTreeContents.SetActive(true);
        placementTipPanel.SetActive(false);
    }


    // Show/hide the UI skill tree
    public void SetVisible(bool isVisible) { 

        if (isPlacementActive && isVisible) {
            return;
        }
        gameObject.SetActive(isVisible);
        if (isVisible) {
            skillTreeContents.SetActive(true);
            placementTipPanel.SetActive(false);
        }
    }
}

