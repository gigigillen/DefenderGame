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
    [SerializeField] private UISelectCanvas selectCanvas;

    private Camera cam;
    private InputActionMap selectingActionMap;
    private InputActionMap skillTreeMap;
    private InputActionMap menuMap;
    private InputAction clickSelectAction;
    private InputAction[] keySelectActions = new InputAction[5];
    private InputAction deselectAction;
    private InputAction toggleSkillTreeAction;
    private InputAction toggleMenuAction;

    private GameObject currentSelectorRing;
    private bool isPointerOverUI;
    private bool isSkillTreeOpen;

    private void Awake() {
        selectingActionMap = inputActions.FindActionMap("Selecting");
        clickSelectAction = selectingActionMap.FindAction("SelectApprentice");
        deselectAction = selectingActionMap.FindAction("DeselectApprentice");

        skillTreeMap = inputActions.FindActionMap("SkillTree");
        toggleSkillTreeAction = skillTreeMap.FindAction("ToggleSkillTree");

        menuMap = inputActions.FindActionMap("Menu");
        toggleMenuAction = menuMap.FindAction("ToggleMenu");

        for (int i=0; i<5; i++) {
            int index = i;
            keySelectActions[i] = selectingActionMap.FindAction($"SelectApprentice{i + 1}");
            keySelectActions[i].performed += _ => SelectApprenticeByNumber(index);
        }

        clickSelectAction.performed += OnSelect;
        deselectAction.performed += OnDeselect;
        toggleSkillTreeAction.performed += ToggleSkillTree;
        toggleMenuAction.performed += ToggleMenu;

        isSkillTreeOpen = false;
        isMenuOpen = false;
    }

    private void OnDestroy() {
        clickSelectAction.performed -= OnSelect;
        deselectAction.performed -= OnDeselect;
        toggleSkillTreeAction.performed -= ToggleSkillTree;
        toggleMenuAction.performed -= ToggleMenu;

        for (int i=0; i<keySelectActions.Length; i++) {
            if (keySelectActions[i] != null) {
                int index = i;
                keySelectActions[i].performed -= _ => SelectApprenticeByNumber(index);
            }
        }

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

    private void OnDeselect(InputAction.CallbackContext context) {
        if (selectedApprentice!=null) {
            DeselectApprentice();
        }
    }

    private void ToggleSkillTree(InputAction.CallbackContext context) {

        if (isMenuOpen) return;

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
            uiSkillTree.SetApprenticeSkills(apprentice.GetApprenticeSkills());

            Vector3 ringPosition = apprentice.transform.position;
            ringPosition.y = 0.1f;
            currentSelectorRing = Instantiate(selectorRingPrefab, ringPosition, Quaternion.identity);
            currentSelectorRing.transform.SetParent(apprentice.transform);

            selectCanvas.SetTargetApprentice(apprentice);
            selectCanvas.gameObject.SetActive(true);

            Debug.Log("Selected Apprentice: " + apprentice.gameObject.name);
            Debug.Log("Skills: " + apprentice.GetApprenticeSkills().GetUnlockedSkills());
        }
        finally {
            isSelectingApprentice = false;
        }
    }

    private void SelectApprenticeByNumber(int index) {
        if (isMenuOpen) return;

        ApprenticeController apprentice = spawnController.GetApprenticeByIndex(index);
        // if same apprentice selected again, deselect
        if (apprentice == selectedApprentice) {
            DeselectApprentice();
        }
        else if (apprentice != null) {
            SelectApprentice(apprentice);
        }
    }

    //deselects apprentice skilltree
    public void DeselectApprentice() {

        if (currentSelectorRing!=null) {
            Destroy(currentSelectorRing);
        }
        selectCanvas.gameObject.SetActive(false);
        selectedApprentice = null;
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
