using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [SerializeField] private ApprenticeType apprenticeType;

    public void OnPointerEnter(PointerEventData eventData) {

        int cost = SpawnApprentice.CalculateSkillPointCost(apprenticeType);
        string description = GetApprenticeDescription();

        TooltipManager.instance.SetAndShowTooltip(
            $"Spawn {apprenticeType} Apprentice",
            apprenticeType,
            description,
            cost,
            false
        );
    }

    public void OnPointerExit(PointerEventData eventData) {

        Debug.Log("pointer exited spawn button");
        TooltipManager.instance.HideToolTip();
    }

    public void OnPointerClick(PointerEventData eventData) {

        TooltipManager.instance.HideToolTip();
    }

    private string GetApprenticeDescription() {

        return apprenticeType switch {
            ApprenticeType.Water => "A medium range apprentice that can slow enemies down. 3 dmg per hit",
            ApprenticeType.Earth => "A high range earth apprentice that launches a rock to deal 5 aoe damage.",
            ApprenticeType.Wind => "A short range apprentice with high attack speed. 2 dmg per hit",
            ApprenticeType.Fire => "A medium range apprentice that deals high damage to single targets. 4 dmg per hit",
            _ => "A simple apprentice that fires bullets at enemies. 2 dmg per hit."
        };
    }
}
