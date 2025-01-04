using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private XPSystem xpSystem;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private ApprenticeType apprenticeType;
    [SerializeField] private string upgradeName;
    [SerializeField] private string message;

    private Button button;


    void Start() {

        button = GetComponent<Button>();
        UpdateButtonState();
    }

    public void OnPointerEnter(PointerEventData eventData) {

        TooltipManager.instance.SetAndShowTooltip(message);
    }

    public void OnPointerExit(PointerEventData eventData) {

        TooltipManager.instance.HideToolTip();
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
