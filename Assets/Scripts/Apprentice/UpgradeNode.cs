using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour {

    [SerializeField] private XPSystem xpSystem;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private ApprenticeType apprenticeType;
    [SerializeField] private string upgradeName;

    private Button button;


    void Start() {

        button = GetComponent<Button>();
        UpdateButtonState();
        
    }

    public void TryPurchaseUpgrade() {

        Debug.Log("trying to purchase upgrade...");

        if (!SkillManager.IsAbilityUnlocked(apprenticeType, upgradeName)) {
            if (xpSystem.SpendSkillPoint()) {
                skillManager.UnlockAbility(apprenticeType, upgradeName);
                button.interactable = false;
            }
        }
    }

    private void UpdateButtonState() {
        button.interactable = !SkillManager.IsAbilityUnlocked(apprenticeType, upgradeName);
    }
}
