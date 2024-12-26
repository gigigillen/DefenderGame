using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SpawnApprentice : MonoBehaviour {

    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap spawningActionMap;
    private InputActionMap selectingActionMap;
    private InputAction spawnAction;
    private InputAction cancelPlacementAction;


    [SerializeField] private ApprenticeTypeData basicTypeData;
    [SerializeField] private ApprenticeTypeData earthTypeData;
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

            Renderer[] renderers = currentPlacingApprentice.GetComponentsInChildren<Renderer>();
            Color newColor = canPlace && hit.collider.CompareTag("Floor") ?
                Color.green :
                Color.red;

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

    private void OnCancelPlacement(InputAction.CallbackContext context) {

        if (currentPlacingApprentice != null) {

            Debug.Log("cancelling placement");
            spawningActionMap.Disable();
            selectingActionMap.Enable();

            Destroy(currentPlacingApprentice);
            currentPlacingApprentice = null;
        }
    }


    public void SetApprenticeTypeToPlace(GameObject apprenticePrefab) {
        if (apprenticeCount >= maxApprentices) {
            Debug.Log("max apprentices reached");
            return;
        }

        if (currentPlacingApprentice != null) {
            Destroy(currentPlacingApprentice);
            currentPlacingApprentice = null;
        }

        this.apprenticePrefab = apprenticePrefab;

        ApprenticeController apprenticeController = apprenticePrefab.GetComponent<ApprenticeController>();
        if (apprenticeController != null) {
            type = apprenticeController.apprenticeType;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 spawnPos = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Floor"))) {
            spawnPos = hit.point;
            spawnPos.y = 0.5f;
        }

        if (apprenticePrefab != null) {
            currentPlacingApprentice = Instantiate(apprenticePrefab, spawnPos, Quaternion.identity);
            Debug.Log("placed preview apprentice!");
            SetupPreviewApprentice(currentPlacingApprentice);

            selectingActionMap.Disable();
            spawningActionMap.Enable();

            Debug.Log("spawning enabled, selecting disabled");
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
        if (col!=null) {
            col.isTrigger = true;
        }

        MonoBehaviour[] scripts = apprentice.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts) {
            if (script!=this) {
                script.enabled = false;
            }
        }
    }


    private void PlaceApprentice(Vector3 position) {

        GameObject finalApprentice = Instantiate(apprenticePrefab, position, Quaternion.identity);
        Debug.Log("placed final apprentice!");
        finalApprentice.transform.SetParent(apprentices);
        apprenticeCount++;
        //GameController.ApprenticesInGame.Add(finalApprentice.GetComponent<ApprenticeController>());
        finalApprentice.name = $"{type} Apprentice {apprenticeCount}";

        GameObject newAttackArea = Instantiate(attackArea, finalApprentice.transform.position, Quaternion.identity);
        newAttackArea.transform.SetParent(finalApprentice.transform);

        Destroy(currentPlacingApprentice);
        currentPlacingApprentice = null;

        spawningActionMap.Disable();
        selectingActionMap.Enable();

        Debug.Log($"{type} apprentice placed at {position}.");
    }


    private GameObject GetApprenticePrefab(ApprenticeType apprenticeType) {

        switch (apprenticeType) {
            case ApprenticeType.Basic: return basicTypeData.apprenticePrefab;
            case ApprenticeType.Earth: return earthTypeData.apprenticePrefab;
            default: return null;
        }
    }


    // decrease apprentice count and hide skill tree on death
    public void killApprentice() {

        apprenticeCount--;
        uiSkillTree.SetVisible(false);

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
        Debug.Log("placement blocking layers" + placementBlockingLayers);
        return hitColliders.Length > 0;
    }
}
