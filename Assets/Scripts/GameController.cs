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

    [SerializeField] private UISkillTree uiSkillTree;
    [SerializeField] private GameObject uiToolBar;
    [SerializeField] private LayerMask attackAreaMask;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject openMenuButton;

    private Camera cam;
    private InputActionMap selectingActionMap;
    private InputAction selectAction;


    private void Awake() {
        selectingActionMap = inputActions.FindActionMap("Selecting");
        selectAction = selectingActionMap.FindAction("SelectApprentice");

        selectAction.performed += OnSelect;
    }

    private void OnDestroy() {
        selectAction.performed -= OnSelect;
    }


    // starts the camera and sets the skilltree to invisible at beginning
    private void Start() {
        cam = Camera.main;
        uiSkillTree.SetVisible(false);

        selectingActionMap.Enable();
    }

    // what the game checks every frame
    private void Update() {
    }


    private void OnSelect(InputAction.CallbackContext context) {

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        int apprenticeLayer = LayerMask.GetMask("Apprentices");

        //checks if the mouse clicked andywhere on the game screen
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, apprenticeLayer)) {
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
            uiSkillTree.SetVisible(true);
            apprentice.GetComponent<Renderer>().material.color = Color.yellow;

            Debug.Log("Selected Apprentice: " + apprentice.gameObject.name);
            Debug.Log("Skills: " + apprentice.GetApprenticeSkills().GetUnlockedSkills());
        }
        finally {
            isSelectingApprentice = false;
        }
    }

    //deselects apprentice skilltree
    public void DeselectApprentice() {
        selectedApprentice.GetComponent<Renderer>().material.color = Color.green;
        selectedApprentice = null;
        uiSkillTree.SetVisible(false);
    }

    public void MenuOpen()
    {
        if (selectedApprentice != null)
        {
            DeselectApprentice();
        }
        menuUI.SetActive(true);
        uiToolBar.SetActive(false);
        openMenuButton.SetActive(false);
        Time.timeScale = 0f;
        isMenuOpen = true;
    }

    public void MenuClose() {
        if (selectedApprentice != null)
        {
            DeselectApprentice();
        }
        menuUI.SetActive(false);
        uiToolBar.SetActive(true);
        openMenuButton.SetActive(true);
        Time.timeScale = 1f;
        isMenuOpen = false;
    }
}
