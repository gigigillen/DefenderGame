using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private XPSystem xpSystem;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private ApprenticeType apprenticeType;
    [SerializeField] private string upgradeName;
    [SerializeField] private int skillPointCost = 1;

    private SpawnApprentice spawnController;
    private Button button;


    void Start() {

        spawnController = FindAnyObjectByType<SpawnApprentice>();
        button = GetComponent<Button>();
        UpdateButtonState();
    }

    public void OnPointerEnter(PointerEventData eventData) {

        bool isUnlocked = SkillManager.IsAbilityUnlocked(apprenticeType, upgradeName);

        TooltipManager.instance.SetAndShowTooltip(
            upgradeName,
            apprenticeType,
            GetUpgradeDescription(apprenticeType),
            skillPointCost,
            isUnlocked
        );
    }

    public void OnPointerExit(PointerEventData eventData) {

        TooltipManager.instance.HideToolTip();
    }

    private string GetUpgradeDescription(ApprenticeType type) {
        return type switch {
            ApprenticeType.Wind =>
                "Creates a vortex on hit, dealing 1 damage per second to enemies within.",
            ApprenticeType.Earth =>
                "Deals two extra instances of crash aoe damage.",
            ApprenticeType.Fire =>
                "Can apply the burning effect to enemies, dealing 1 damage per second.",
            ApprenticeType.Water =>
                "Can apply the wet effect to enemies, slowing them down.",
            _ => "No description available."
        };
    }

    public void TryPurchaseUpgrade() {

        Debug.Log("trying to purchase upgrade...");

        if (!SkillManager.IsAbilityUnlocked(apprenticeType, upgradeName)) {
            if (xpSystem.GetSkillPoints() >= skillPointCost) {
                bool success = SpendRequiredSkillPoints();
                if (success) {
                    skillManager.UnlockAbility(apprenticeType, upgradeName);
                    button.interactable = false;
                    spawnController.UpdateAllSpawnCosts();
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
