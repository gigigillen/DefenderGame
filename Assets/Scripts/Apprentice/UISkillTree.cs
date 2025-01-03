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




    // Show/hide the UI skill tree
    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}

