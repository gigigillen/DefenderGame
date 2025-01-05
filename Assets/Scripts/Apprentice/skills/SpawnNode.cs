using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private ApprenticeType apprenticeType;

    public void OnPointerEnter(PointerEventData eventData) {

        int currentCost = SpawnApprentice.CalculateSkillPointCost(apprenticeType);
        string description = GetApprenticeDescription();

        TooltipManager.instance.SetAndShowTooltip(
            $"Spawn {apprenticeType} Apprentice",
            apprenticeType,
            description,
            currentCost,
            false
        );
    }

    public void OnPointerExit(PointerEventData eventData) {

        TooltipManager.instance.HideToolTip();
    }

    private string GetApprenticeDescription() {

        return apprenticeType switch {
            ApprenticeType.Water => "A water apprentice that can slow enemies with its attacks.",
            ApprenticeType.Earth => "An earth apprentice that launches a rock to deal aoe damage.",
            ApprenticeType.Wind => "A wind apprentice with high attack speed.",
            ApprenticeType.Fire => "A fire apprentice that deals high damage to single targets.",
            _ => "A basic apprentice that fires bullets at enemies."
        };
    }
}
