using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
    // makes GameController a singleton by calling itself
    public static GameController instance;

    public ApprenticeController selectedApprentice;
    public bool isMenuOpen = false;
    private bool isSelectingApprentice = false;

    [SerializeField] private GameObject selectorRingPrefab;
    [SerializeField] private UISkillTree uiSkillTree;
    [SerializeField] private LayerMask attackAreaMask;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject openMenuButton;
    [SerializeField] private SpawnApprentice spawnController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private UISelectCanvas selectCanvas;

    private Camera cam;
    private InputActionMap selectingActionMap;
    private InputActionMap skillTreeMap;
    private InputActionMap menuMap;
    private InputAction clickSelectAction;
    private InputAction cycleApprenticeAction;
    private InputAction deselectApprenticeAction;
    private InputAction toggleSkillTreeAction;
    private InputAction toggleMenuAction;

    private GameObject currentSelectorRing;
    private int currentApprenticeIndex = -1;
    private bool isPointerOverUI;
    private bool isSkillTreeOpen;

    private void Awake() {
        selectingActionMap = inputActions.FindActionMap("Selecting");
        clickSelectAction = selectingActionMap.FindAction("SelectApprentice");
        deselectApprenticeAction = selectingActionMap.FindAction("DeselectApprentice");
        cycleApprenticeAction = selectingActionMap.FindAction("CycleApprentice");

        skillTreeMap = inputActions.FindActionMap("SkillTree");
        toggleSkillTreeAction = skillTreeMap.FindAction("ToggleSkillTree");

        menuMap = inputActions.FindActionMap("Menu");
        toggleMenuAction = menuMap.FindAction("ToggleMenu");

        clickSelectAction.performed += OnSelect;
        deselectApprenticeAction.performed += OnDeselect;
        cycleApprenticeAction.performed += OnCycleApprentice;
        toggleSkillTreeAction.performed += ToggleSkillTree;
        toggleMenuAction.performed += ToggleMenu;

        isSkillTreeOpen = false;
        isMenuOpen = false;
    }

    private void OnDestroy() {
        clickSelectAction.performed -= OnSelect;
        deselectApprenticeAction.performed -= OnDeselect;
        cycleApprenticeAction.performed -= OnCycleApprentice;
        toggleSkillTreeAction.performed -= ToggleSkillTree;
        toggleMenuAction.performed -= ToggleMenu;

        Time.timeScale = 1f;
    }


    // starts the camera and sets the skilltree to invisible at beginning
    private void Start() {
        cam = Camera.main;
        uiSkillTree.SetVisible(false);

        selectingActionMap.Enable();
        skillTreeMap.Enable();
        menuMap.Enable();
    }

    // what the game checks every frame
    private void Update() {
       isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }


    private void OnSelect(InputAction.CallbackContext context) {

        // if over UI as of this frame’s update, skip the raycast
        if (isPointerOverUI) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        int apprenticeLayer = LayerMask.GetMask("Apprentices");

        //checks if the mouse clicked anywhere on the game screen
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            //checks if an apprentice was clicked and uses method SelectedApprentice
            ApprenticeController clickedApprentice = hit.collider.GetComponent<ApprenticeController>();
            if (clickedApprentice != null) {
                SelectApprentice(clickedApprentice);
            }
            else if (selectedApprentice != null) {
                DeselectApprentice();
            }
        }
    }

    private void OnCycleApprentice(InputAction.CallbackContext context) {

        if (isMenuOpen) return;

        List<ApprenticeController> apprentices = spawnController.GetActiveApprentices();
        if (apprentices.Count == 0) return;

        if (selectedApprentice == null) {
            currentApprenticeIndex = 0;
            SelectApprentice(apprentices[currentApprenticeIndex]);
            return;
        }

        currentApprenticeIndex = (currentApprenticeIndex + 1) % apprentices.Count;
        SelectApprentice(apprentices[currentApprenticeIndex]);
    }


    private void OnDeselect(InputAction.CallbackContext context) {
        if (selectedApprentice!=null) {
            DeselectApprentice();
        }
    }

    private void ToggleSkillTree(InputAction.CallbackContext context) {

        if (isMenuOpen) return;

        if (spawnController.isPlacingApprentice) {
            // while placing an apprentice the placement tip appears in the skill tree
            // skill tree cannot close during placement
            return;
        }

        isSkillTreeOpen = !isSkillTreeOpen;
        uiSkillTree.SetVisible(isSkillTreeOpen);
    }

    private void ToggleMenu(InputAction.CallbackContext context) {

        if (gameOverUI != null && gameOverUI.activeSelf) return;

        if (isMenuOpen) {
            MenuClose();
        }
        else {
            MenuOpen();
        }
    }

    // pauses all the game physics and puts a game over scene
    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        // makes sure there is no skill tree on screen for gameover screen
        if (selectedApprentice != null)
        {
            DeselectApprentice();
        }
    }

    // rebuilds the game index again and resets the physics of the game
    public void PlayAgain()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // selects an apprentice and puts up its skilltree ui
    public void SelectApprentice(ApprenticeController apprentice) {

        if (isSelectingApprentice) return;

        isSelectingApprentice = true;
        try {
            if (selectedApprentice != null) {
                DeselectApprentice();
            }

            // finds which apprentice and fetches their skilltree from their method
            selectedApprentice = apprentice;

            List<ApprenticeController> apprentices = spawnController.GetActiveApprentices();
            currentApprenticeIndex = apprentices.IndexOf(apprentice);

            Vector3 ringPosition = apprentice.transform.position;
            ringPosition.y = 0.1f;
            currentSelectorRing = Instantiate(selectorRingPrefab, ringPosition, Quaternion.identity);
            currentSelectorRing.transform.SetParent(apprentice.transform);

            // move the camera to the selected apprentice
            if (cameraController != null) {
                cameraController.MoveToApprentice(apprentice.transform.position, 1f);
            }

            selectCanvas.SetTargetApprentice(apprentice);
            selectCanvas.gameObject.SetActive(true);

            Debug.Log("Selected Apprentice: " + apprentice.gameObject.name);
        }
        finally {
            isSelectingApprentice = false;
        }
    }

 
    //deselects apprentice skilltree
    public void DeselectApprentice() {

        if (currentSelectorRing!=null) {
            Destroy(currentSelectorRing);
        }
        selectCanvas.gameObject.SetActive(false);
        selectedApprentice = null;
        currentApprenticeIndex = -1;
    }

    // called when menu is toggled open
    public void MenuOpen()
    {
        if (selectedApprentice != null)
        {
            DeselectApprentice();
        }

        uiSkillTree.SetVisible(false);
        menuUI.SetActive(true);
        openMenuButton.SetActive(false);
        Time.timeScale = 0f;
        isMenuOpen = true;
    }

    // called when menu is toggled close
    public void MenuClose() {

        if (selectedApprentice != null)
        {
            DeselectApprentice();
        }
        menuUI.SetActive(false);
        openMenuButton.SetActive(true);
        Time.timeScale = 1f;
        isMenuOpen = false;
    }


    // called when options button is clicked
    public void OnOptionsClick() {

        // make the options panel visible
        // set the rest of the menu not visible
    }

    // called when main menu button is clicked
    public void OnMainMenuClick() {

        SceneManager.LoadScene(0);
    }

    // called when quit button is clicked
    public void OnQuitClick() {

        Application.Quit();
    }
}
