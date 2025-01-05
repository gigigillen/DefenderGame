using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SpawnApprentice : MonoBehaviour {

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private XPSystem xpSystem; // Reference to XPSystem
    private InputActionMap spawningActionMap;
    private InputActionMap selectingActionMap;
    private InputAction spawnAction;
    private InputAction cancelPlacementAction;
    private InputAction[] quickSpawnActions = new InputAction[5];

    [SerializeField] private ApprenticeTypeData[] typeDatas = new ApprenticeTypeData[5];
    [SerializeField] private LayerMask placementBlockingLayers;
    [SerializeField] private Transform apprentices;
    [SerializeField] private UISkillTree uiSkillTree;
    [SerializeField] private TextMeshProUGUI apprenticeText;
    [SerializeField] private TextMeshProUGUI[] spawnCostText = new TextMeshProUGUI[4];

    public ApprenticeType type = ApprenticeType.Basic; // default type basic
    public bool isPlacingApprentice = false;

    private GameObject currentPlacingApprentice;
    private Camera cam;
    private GameController gameController;
    private int apprenticeCount;
    private const int maxApprentices = 10;
    private bool canPlace = false;
    private GameObject apprenticePrefab;
    private List<ApprenticeController> activeApprentices = new List<ApprenticeController>();
    private int pendingSkillPointCost = 0;


    private void Awake() {

        spawningActionMap = inputActions.FindActionMap("Spawning");
        selectingActionMap = inputActions.FindActionMap("Selecting");

        spawnAction = spawningActionMap.FindAction("PlaceApprentice");
        cancelPlacementAction = spawningActionMap.FindAction("CancelPlacement");

        for (int i = 0; i < 5; i++) {
            int index = i;
            quickSpawnActions[i] = selectingActionMap.FindAction($"Spawn{GetApprenticeTypeName(i)}{i+1}");
            quickSpawnActions[i].performed += ctx => OnSpawnKey(index);
        }

        spawnAction.performed += OnSpawnPerformed;
        cancelPlacementAction.performed += OnCancelPlacement;
    }


    void Start() {

        cam = Camera.main;
        gameController = FindAnyObjectByType<GameController>();
        apprenticeCount = 0;
        apprenticeText.text = "";
        SetApprenticeText();
        UpdateAllSpawnCosts();

        selectingActionMap.Enable();
        spawningActionMap.Disable();
    }


    private void FixedUpdate() {

        // update apprentice count in UI
        SetApprenticeText();

        if (currentPlacingApprentice != null) {
            HandlePlacement();
        }
    }


    void HandlePlacement() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            Vector3 position = hit.point;
            position.y = 0.5f;

            currentPlacingApprentice.transform.position = position;
            currentPlacingApprentice.transform.rotation = Quaternion.identity;

            Ray downRay = new Ray(position + Vector3.up * 2f, Vector3.down);
            RaycastHit pathHit;

            if (Physics.Raycast(downRay, out pathHit, 4f, LayerMask.GetMask("ApprenticePath"))) {
                bool noOverlap = !IsOverlappingObjects(position);
                canPlace = noOverlap;
            }
            else {
                canPlace = false;
            }

            Renderer[] renderers = currentPlacingApprentice.GetComponentsInChildren<Renderer>();
            Color newColor = canPlace ? Color.green : Color.red;

            foreach (Renderer renderer in renderers) {
                renderer.material.color = newColor;
            }
        }
    }


    private void OnSpawnPerformed(InputAction.CallbackContext context) {

        if (currentPlacingApprentice != null && canPlace) {
            PlaceApprentice(currentPlacingApprentice.transform.position);
        }
    }

    private void OnSpawnKey(int index) {
        Debug.Log("trying to spawn with hotkey");
        if (isPlacingApprentice || gameController.isMenuOpen) return;

        ApprenticeTypeData selectedType = index switch {
            0 => typeDatas[0],
            1 => typeDatas[1],
            2 => typeDatas[2],
            3 => typeDatas[3],
            4 => typeDatas[4],
            _ => null
        };

        if (selectedType != null && selectedType.apprenticePrefab != null) {
            SetApprenticeTypeToPlace(selectedType.apprenticePrefab);
        }
    }

    private void OnCancelPlacement(InputAction.CallbackContext context) {

        CancelPlacement();
    }


    public void SetApprenticeTypeToPlace(GameObject apprenticePrefab)
    {
        // Dynamically assign XPSystem if it's null
        if (xpSystem == null)
        {
            xpSystem = XPSystem.FindFirstObjectByType<XPSystem>();
            if (xpSystem == null)
            {
                Debug.LogError("XPSystem reference is null! Ensure an XPSystem is in the scene.");
                return;
            }
        }

        if (apprenticeCount >= maxApprentices)
        {
            Debug.Log("Max apprentices reached");
            return;
        }

        ApprenticeController apprenticeController = apprenticePrefab.GetComponent<ApprenticeController>();
        if (apprenticeController != null)
        {
            pendingSkillPointCost = CalculateSkillPointCost(apprenticeController.apprenticeType);

            // Check if the player has enough skill points
            if (xpSystem.GetSkillPoints() < pendingSkillPointCost)
            {
                Debug.Log($"Not enough skill points to place {apprenticeController.apprenticeType} apprentice. Required: {pendingSkillPointCost}, Available: {xpSystem.GetSkillPoints()}");
                return;
            }

            this.apprenticePrefab = apprenticePrefab;
            type = apprenticeController.apprenticeType;
            CreateApprenticePreview(apprenticePrefab, apprenticeController);
            isPlacingApprentice = true;
        }
    }

    private int CalculateSkillPointCost(ApprenticeType type) {

        int cost = (type == ApprenticeType.Basic) ? 1 : 2;
        switch (type) {
            case ApprenticeType.Wind:
                if (SkillManager.IsAbilityUnlocked(ApprenticeType.Wind, "Vortex")) {
                    cost += 1;
                }
                break;
            case ApprenticeType.Earth:
                if (SkillManager.IsAbilityUnlocked(ApprenticeType.Earth, "Rocky Pulse")) {
                    cost += 1;
                }
                break;
            case ApprenticeType.Fire:
                if (SkillManager.IsAbilityUnlocked(ApprenticeType.Fire, "Burning")) {
                    cost += 1;
                }
                break;
            case ApprenticeType.Water:
                if (SkillManager.IsAbilityUnlocked(ApprenticeType.Water, "Wetness")) {
                    cost += 1;
                }
                break;
        }
        return cost;
    }

    public void UpdateAllSpawnCosts() {

        for (int i = 1; i < 5; i++) {
            ApprenticeType type = typeDatas[i].type;
            int cost = CalculateSkillPointCost(type);
            spawnCostText[i-1].text = cost.ToString();
        }
    }


    private void SetupPreviewApprentice(GameObject apprentice) {

        apprentice.layer = LayerMask.NameToLayer("ApprenticePreview");
        Rigidbody rb = apprentice.GetComponent<Rigidbody>();
        if (rb!=null) {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Collider col = apprentice.GetComponent<Collider>();
        if (col != null) {
            col.isTrigger = true;
        }

        MonoBehaviour[] scripts = apprentice.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts) {
            if (script != this) {
                script.enabled = false;
            }
        }
    }


    private void CreateApprenticePreview(GameObject apprenticePrefab, ApprenticeController controller) {

        if (currentPlacingApprentice != null) {
            CancelPlacement();
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 spawnPos = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("ApprenticePath"))) {
            spawnPos = hit.point;
            spawnPos.y = 0.5f;
        }

        currentPlacingApprentice = Instantiate(apprenticePrefab, spawnPos, Quaternion.identity);
        SetupPreviewApprentice(currentPlacingApprentice);

        selectingActionMap.Disable();
        spawningActionMap.Enable();

        uiSkillTree.OnPlacementStart();
    }


    private void PlaceApprentice(Vector3 position) {

        for (int i = 0; i < pendingSkillPointCost; i++) {
            xpSystem.SpendSkillPoint();
        }

        GameObject finalApprentice = Instantiate(apprenticePrefab, position, Quaternion.identity);
        finalApprentice.layer = LayerMask.NameToLayer("Apprentices");

        ApprenticeController apprenticeController = finalApprentice.GetComponent<ApprenticeController>();
        activeApprentices.Add(apprenticeController);

        finalApprentice.transform.SetParent(apprentices);
        apprenticeCount++;
        finalApprentice.name = $"{type} Apprentice {apprenticeCount}";

        CleanupPlacement();
        gameController.SelectApprentice(apprenticeController);

        Debug.Log($"{type} apprentice placed at {position}.");
    }


    private string GetApprenticeTypeName(int index) {
        return index switch {
            0 => "Basic",
            1 => "Wind",
            2 => "Water",
            3 => "Earth",
            4 => "Fire",
            _ => string.Empty
        };
    }


    private void CleanupPlacement() {

        if (currentPlacingApprentice != null) {
            Destroy(currentPlacingApprentice);
            currentPlacingApprentice = null;
        }

        isPlacingApprentice = false;
        pendingSkillPointCost = 0;
        spawningActionMap.Disable();
        selectingActionMap.Enable();
        uiSkillTree.OnPlacementEnd();
    }


    public void CancelPlacement() {

        if (isPlacingApprentice) {
            Debug.Log("Cancelled placement - no skill points spent");
            CleanupPlacement();
        }
    }
  

    // decrease apprentice count and remove from active apprentices list
    public void RemoveApprentice(ApprenticeController apprentice) {

        activeApprentices.Remove(apprentice);
        apprenticeCount--;
    }


    public List<ApprenticeController> GetActiveApprentices() {
        return new List<ApprenticeController>(activeApprentices);
    }


    // update apprentice count in UI
    private void SetApprenticeText() {

        apprenticeText.text = $"{apprenticeCount}/{maxApprentices}";
    }


    private bool IsOverlappingObjects(Vector3 position) {
        float checkRadius = 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius, placementBlockingLayers);
        return hitColliders.Length > 0;
    }


    private void OnDestroy() {
        spawnAction.performed -= OnSpawnPerformed;
        cancelPlacementAction.performed -= OnCancelPlacement;
    }
}
