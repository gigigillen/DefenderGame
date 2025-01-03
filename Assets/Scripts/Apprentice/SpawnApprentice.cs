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
    


    [SerializeField] private LayerMask placementBlockingLayers;
    [SerializeField] private GameObject attackArea; // the attack area prefab
    [SerializeField] private Transform apprentices;
    [SerializeField] private UISkillTree uiSkillTree;
    [SerializeField] private TextMeshProUGUI apprenticeText;

    public ApprenticeType type = ApprenticeType.Basic; // default type basic

    private GameObject currentPlacingApprentice;
    private Camera cam;
    private int apprenticeCount;
    private const int maxApprentices = 5; // max apprentices is immutable
    private bool canPlace = false;
    private GameController gameController;
    private GameObject apprenticePrefab;
    private List<ApprenticeController> activeApprentices = new List<ApprenticeController>();


    private void Awake() {

        spawningActionMap = inputActions.FindActionMap("Spawning");
        selectingActionMap = inputActions.FindActionMap("Selecting");
        spawnAction = spawningActionMap.FindAction("PlaceApprentice");
        cancelPlacementAction = spawningActionMap.FindAction("CancelPlacement");

        spawnAction.performed += OnSpawnPerformed;
        cancelPlacementAction.performed += OnCancelPlacement;
    }


    private void OnDestroy() {
        spawnAction.performed -= OnSpawnPerformed;
    }


    void Start() {

        cam = Camera.main;
        apprenticeCount = 0;
        apprenticeText.text = "";
        SetApprenticeText();
        gameController = FindAnyObjectByType<GameController>();

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

        int layerMask = LayerMask.GetMask("Floor");

        if (Physics.Raycast(ray, out hit, 100f, layerMask)) {
            Vector3 position = hit.point;
            position.y = 0.5f;

            currentPlacingApprentice.transform.position = position;
            currentPlacingApprentice.transform.rotation = Quaternion.identity;

            canPlace = IsWithinBounds(position) && !IsOverlappingObjects(position) ;
        }
    }


    private void OnSpawnPerformed(InputAction.CallbackContext context) {

        if (currentPlacingApprentice != null && canPlace) {
            PlaceApprentice(currentPlacingApprentice.transform.position);
        }
    }

    private void OnCancelPlacement(InputAction.CallbackContext context) {

        if (currentPlacingApprentice != null) {

            Debug.Log("cancelling placement");
            spawningActionMap.Disable();
            selectingActionMap.Enable();

            Destroy(currentPlacingApprentice);
            currentPlacingApprentice = null;
        }
    }


   public void SetApprenticeTypeToPlace(GameObject apprenticePrefab)
    {
        // Dynamically assign XPSystem if it's null
        if (xpSystem == null)
        {
            xpSystem = FindObjectOfType<XPSystem>();
            if (xpSystem == null)
            {
                Debug.LogError("XPSystem reference is null! Ensure an XPSystem is in the scene.");
                return;
            }
        }

        ApprenticeController apprenticeController = apprenticePrefab.GetComponent<ApprenticeController>();
        if (apprenticeController != null)
        {
            int requiredSkillPoints = 0; // Default required skill points

            // Check for apprentices that require more skill points
            switch (apprenticeController.apprenticeType)
            {
                case ApprenticeType.Water:
                case ApprenticeType.Earth:
                requiredSkillPoints = 1;
                break;
                case ApprenticeType.Wind:
                case ApprenticeType.Fire:
                    requiredSkillPoints = 2;
                    break;
            }

            // Check if the player has enough skill points
            if (xpSystem.GetSkillPoints() < requiredSkillPoints)
            {
                Debug.Log($"Not enough skill points to place {apprenticeController.apprenticeType} apprentice. Required: {requiredSkillPoints}, Available: {xpSystem.GetSkillPoints()}");
                return;
            }

            // Spend the required skill points
            for (int i = 0; i < requiredSkillPoints; i++)
            {
                xpSystem.SpendSkillPoint();
            }

            Debug.Log($"{apprenticeController.apprenticeType} apprentice unlocked! Remaining skill points: {xpSystem.GetSkillPoints()}");
        }

        if (apprenticeCount >= maxApprentices)
        {
            Debug.Log("Max apprentices reached");
            return;
        }

        if (currentPlacingApprentice != null)
        {
            Destroy(currentPlacingApprentice);
            currentPlacingApprentice = null;
        }

        this.apprenticePrefab = apprenticePrefab;

        if (apprenticeController != null)
        {
            type = apprenticeController.apprenticeType;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 spawnPos = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Floor")))
        {
            spawnPos = hit.point;
            spawnPos.y = 0.5f;
        }

        if (apprenticePrefab != null)
        {
            currentPlacingApprentice = Instantiate(apprenticePrefab, spawnPos, Quaternion.identity);
            Debug.Log($"Placed preview apprentice of type {apprenticeController?.apprenticeType}!");
            SetupPreviewApprentice(currentPlacingApprentice);

            selectingActionMap.Disable();
            spawningActionMap.Enable();

            Debug.Log("Spawning enabled, selecting disabled");
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


    private void PlaceApprentice(Vector3 position) {

        GameObject finalApprentice = Instantiate(apprenticePrefab, position, Quaternion.identity);
        finalApprentice.layer = LayerMask.NameToLayer("Apprentices");

        ApprenticeController apprenticeController = finalApprentice.GetComponent<ApprenticeController>();
        activeApprentices.Add(apprenticeController);

        finalApprentice.transform.SetParent(apprentices);
        apprenticeCount++;
        finalApprentice.name = $"{type} Apprentice {apprenticeCount}";

        GameObject newAttackArea = Instantiate(attackArea, finalApprentice.transform.position, Quaternion.identity);
        newAttackArea.transform.SetParent(finalApprentice.transform);

        Destroy(currentPlacingApprentice);
        currentPlacingApprentice = null;

        spawningActionMap.Disable();
        selectingActionMap.Enable();

        Debug.Log($"{type} apprentice placed at {position}.");
    }


    // decrease apprentice count and hide skill tree on death
    public void RemoveApprentice(ApprenticeController apprentice) {

        activeApprentices.Remove(apprentice);
        apprenticeCount--;
        uiSkillTree.SetVisible(false);

    }

    public ApprenticeController GetApprenticeByIndex(int index) {
        if (index>= 0 && index < activeApprentices.Count) {
            return activeApprentices[index];
        }
        return null;
    }


    // update apprentice count in UI
    private void SetApprenticeText() {

        apprenticeText.text = $"Apprentices: {apprenticeCount}/{maxApprentices}";
    }


    private bool IsWithinBounds(Vector3 position) {
        return position.x >= -9f && position.x <= 9f &&
               position.z >= -9f && position.z <= 9f;
    }


    private bool IsOverlappingObjects(Vector3 position) {
        float checkRadius = 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius, placementBlockingLayers);
        return hitColliders.Length > 0;
    }
}
