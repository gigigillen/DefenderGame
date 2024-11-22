using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SpawnApprentice : MonoBehaviour {

    public InputActionAsset inputActions;
    private InputActionMap spawningActionMap;
    private InputActionMap selectingActionMap;

    public GameObject basicApprenticePrefab; // the apprentice prefab to instantiate
    public GameObject earthApprenticePrefab;
    public ApprenticeType type = ApprenticeType.Basic; // default type basic

    public GameObject attackArea; // the attack area prefab

    private GameObject CurrentPlacingApprentice;

    private Camera cam;

    private int apprenticeCount;
    private const int maxApprentices = 5; // max apprentices is immutable
    public TextMeshProUGUI apprenticeText;

    [SerializeField] private Transform apprentices;
    private GameController gameController;

    [SerializeField] private UISkillTree uiSkillTree;

    private bool canPlace = false;

    private void Awake() {

        spawningActionMap = inputActions.FindActionMap("Spawning");
        selectingActionMap = inputActions.FindActionMap("Selecting");
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

        if (CurrentPlacingApprentice != null) {
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
            CurrentPlacingApprentice.transform.position = position;
            CurrentPlacingApprentice.transform.rotation = Quaternion.identity;

            canPlace = IsWithinBounds(position);

            if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && hit.collider.CompareTag("Floor")) {

                PlaceApprentice(position);
            }
        }
    }

    private bool IsWithinBounds(Vector3 position) {
        return position.x >= -9f && position.x <= 9f &&
               position.z >= -9f && position.z <= 9f;
    }

    public void SetApprenticeTypeToPlace(GameObject apprenticePrefab) {
        if (apprenticeCount >= maxApprentices) {
            Debug.Log("max apprentices reached");
            return;
        }

        ApprenticeController apprenticeController = apprenticePrefab.GetComponent<ApprenticeController>();
        if (apprenticeController != null) {
            type = apprenticeController.apprenticeType;
        }

        if (CurrentPlacingApprentice != null) {
            Destroy(CurrentPlacingApprentice);
            CurrentPlacingApprentice = null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 spawnPos = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Floor"))) {
            spawnPos = hit.point;
            spawnPos.y = 0.5f;
        }

        if (apprenticePrefab != null) {
            CurrentPlacingApprentice = Instantiate(apprenticePrefab, spawnPos, Quaternion.identity);
            Debug.Log("placed preview apprentice!");
            SetupPreviewApprentice(CurrentPlacingApprentice);

            selectingActionMap.Disable();
            spawningActionMap.Enable();
        }

    }

    void OnSpawn() {


        //// spawn an apprentice only if within maxApprentices 
        //if (apprenticeCount < maxApprentices && Mouse.current.leftButton.wasPressedThisFrame) { 
        //    Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit)) {
        //        if (gameController.selectedApprentice == null)
        //        {
        //            Vector3 spawnPosition = new Vector3(hit.point.x, 0.5f, hit.point.z);

        //            // check first if the attempted spawn position is within bounds of the floor
        //            if (spawnPosition.x >= -9f && spawnPosition.x <= 9f && spawnPosition.z >= -9f && spawnPosition.z <= 9f && spawnPosition.y == 0.5)
        //            {

        //                // instantiate the apprentice prefab
        //                GameObject newApprentice = Instantiate(CurrentPlacingTower, spawnPosition, Quaternion.identity);
        //                newApprentice.transform.SetParent(apprentices);

        //                newApprentice.name = "Apprentice " + (apprenticeCount + 1);
        //                apprenticeCount++;

        //                // instantiate the apprentice's attack area and set it as its child
        //                GameObject newAttackArea = Instantiate(attackArea, newApprentice.transform.position, Quaternion.identity);
        //                newAttackArea.transform.SetParent(newApprentice.transform);

        //                ApprenticeController apprenticeController = newApprentice.GetComponent<ApprenticeController>();

        //                Debug.Log("New apprentice spawned with skills system");
        //            }
        //        }
        //    }
        //}
    }

    private void SetupPreviewApprentice(GameObject apprentice) {
        Rigidbody rb = apprentice.GetComponent<Rigidbody>();
        if (rb!=null) {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        MonoBehaviour[] scripts = apprentice.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts) {
            if (script!=this) {
                script.enabled = false;
            }
        }
    }

    private void PlaceApprentice(Vector3 position) {

        GameObject finalApprentice = Instantiate(GetApprenticePrefab(type), position, Quaternion.identity);
        Debug.Log("placed final apprentice!");
        finalApprentice.transform.SetParent(apprentices);
        apprenticeCount++;
        finalApprentice.name = $"{type} Apprentice {apprenticeCount}";

        GameObject newAttackArea = Instantiate(attackArea, finalApprentice.transform.position, Quaternion.identity);
        newAttackArea.transform.SetParent(finalApprentice.transform);

        Destroy(CurrentPlacingApprentice);
        CurrentPlacingApprentice = null;

        spawningActionMap.Disable();
        selectingActionMap.Enable();

        Debug.Log($"{type} apprentice placed at {position}.");
    }

    //public void SetApprenticeToPlace(GameObject apprenticePrefab) {

    //    ApprenticeController apprenticeController = apprenticePrefab.GetComponent<ApprenticeController>();

    //    if (apprenticeController != null) {
    //        type = apprenticeController.apprenticeType;
    //    }

    //    CurrentPlacingApprentice = Instantiate(apprenticePrefab, Vector3.zero, Quaternion.identity);
    //}


    private GameObject GetApprenticePrefab(ApprenticeType apprenticeType) {

        switch (apprenticeType) {
            case ApprenticeType.Basic: return basicApprenticePrefab;
            case ApprenticeType.Earth: return earthApprenticePrefab;
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
}
