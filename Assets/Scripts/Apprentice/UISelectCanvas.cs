using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class UISelectCanvas : MonoBehaviour {

    private Camera mainCamera;
    [SerializeField] private RectTransform baseRectTransform;
    [SerializeField] private TextMeshProUGUI apprenticeTypeText;
    [SerializeField] private TextMeshProUGUI upgradesText;
    [SerializeField] private TextMeshProUGUI refundText;
    [SerializeField] private InputActionAsset inputActions;

    private InputActionMap selectingActionMap;
    private InputAction sellAction;

    private ApprenticeController currentApprentice;
    private XPSystem xpSystem;
    private SpawnApprentice spawnController;
    private GameController gameController;


    private void Awake() {

        mainCamera = Camera.main;
        gameObject.SetActive(false);
        xpSystem = FindFirstObjectByType<XPSystem>();
        spawnController = FindFirstObjectByType<SpawnApprentice>();
        gameController = FindFirstObjectByType<GameController>();

        selectingActionMap = inputActions.FindActionMap("Selecting");
        sellAction = selectingActionMap.FindAction("SellApprentice");
        sellAction.performed += OnSellActionPerformed;
    }

    private void OnDestroy() {

        sellAction.performed -= OnSellActionPerformed;
    }

    private void OnSellActionPerformed(InputAction.CallbackContext context) {

        if (!gameController.isMenuOpen &&
            currentApprentice != null &&
            gameController.selectedApprentice == currentApprentice &&
            gameObject.activeSelf) {
            SellApprentice();
        }
    }

    public void SetTargetApprentice(ApprenticeController apprentice) {

        currentApprentice = apprentice;
        UpdateUIContents();
    }

    public void UpdateUIContents() {

        if (currentApprentice != null) {

            apprenticeTypeText.text = $"Type: <color={TooltipManager.GetTypeColor(currentApprentice.apprenticeType)}>{currentApprentice.apprenticeType}</color></size>\n";

            List<string> unlockedAbilities = SkillManager.GetUnlockedAbilities(currentApprentice.apprenticeType);
            string upgradeListText = unlockedAbilities.Count > 0
                ? string.Join(", ", unlockedAbilities)
                : "None";
            upgradesText.text = $"Upgrades: {upgradeListText}";

            int returnValue = CalculateReturnValue(currentApprentice.apprenticeType);
            refundText.text = $"Sell ({returnValue} SP)";
        }
    }


    public void SellApprentice() {

        if (currentApprentice != null) {
            int returnValue = CalculateReturnValue(currentApprentice.apprenticeType);
            for (int i = 0; i < returnValue; i++) {
                xpSystem.AddXP(100);
            }

            spawnController.RemoveApprentice(currentApprentice);
            gameController.DeselectApprentice();
            Destroy(currentApprentice.gameObject);
        }
    }

    private int CalculateReturnValue(ApprenticeType type) {

        // refund 50% of original cost
        int originalCost = type switch {
            ApprenticeType.Water => 2,
            ApprenticeType.Earth => 2,
            ApprenticeType.Wind => 3,
            ApprenticeType.Fire => 3,
            _ => 1
        };
        return Mathf.Max(1, originalCost / 2); // cant refund less than 1
    }

    private void LateUpdate() {

        if (currentApprentice != null) {
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(currentApprentice.transform.position);
            screenPoint.x -= 100f;
            screenPoint.y += 50f;
            baseRectTransform.position = screenPoint;
        }
    }
}
