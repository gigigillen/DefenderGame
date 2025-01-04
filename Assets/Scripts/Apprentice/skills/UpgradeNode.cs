using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private XPSystem xpSystem;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private ApprenticeType apprenticeType;
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private int skillPointCost = 1;

    private Button button;


    void Start() {

        button = GetComponent<Button>();
        UpdateButtonState();
    }

    public void OnPointerEnter(PointerEventData eventData) {

        TooltipManager.instance.SetAndShowTooltip(
            upgradeName,
            apprenticeType,
            description,
            skillPointCost
        );
    }

    public void OnPointerExit(PointerEventData eventData) {

        TooltipManager.instance.HideToolTip();
    }

    public void TryPurchaseUpgrade() {

        Debug.Log("trying to purchase upgrade...");

        if (!SkillManager.IsAbilityUnlocked(apprenticeType, upgradeName)) {
            if (xpSystem.GetSkillPoints() >= skillPointCost) {
                bool success = SpendRequiredSkillPoints();
                if (success) {
                    skillManager.UnlockAbility(apprenticeType, upgradeName);
                    button.interactable = false;
                }
            }
        }
    }

    private bool SpendRequiredSkillPoints() {

        for (int i=0; i< skillPointCost; i++) {
            if (!xpSystem.SpendSkillPoint()) {
                return false;
            }
        }
        return true;
    }
 
    private void UpdateButtonState() {
        button.interactable = !SkillManager.IsAbilityUnlocked(apprenticeType, upgradeName);
    }
}
