using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {

    public static GameController instance;

    [SerializeField] private UISkillTree uiSkillTree;

    public ApprenticeController selectedApprentice;

    private Camera cam;

    [SerializeField] private LayerMask attackAreaMask;

    [SerializeField] private GameObject gameOverUI;

    private void Start() {

        cam = Camera.main;
        uiSkillTree.SetVisible(false);
    }

    private void Update() {

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~attackAreaMask)) {
                ApprenticeController clickedApprentice = hit.collider.GetComponent<ApprenticeController>();
                if (clickedApprentice != null) {
                    SelectApprentice(clickedApprentice);
                }
                else {
                    if (selectedApprentice != null)
                    {
                        DeselectApprentice();
                    }
                }
            }
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        if (selectedApprentice != null)
        {
            DeselectApprentice();
        }
    }

    public void PlayAgain()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SelectApprentice(ApprenticeController apprentice) {

        if (selectedApprentice != null) {
            DeselectApprentice();
        }

        selectedApprentice = apprentice;
        uiSkillTree.SetApprenticeSkills(apprentice.GetApprenticeSkills());
        uiSkillTree.SetVisible(true);
        apprentice.GetComponent<Renderer>().material.color = Color.yellow;

        Debug.Log("Selected Apprentice: " + apprentice.gameObject.name);
        Debug.Log("Skills: " + apprentice.GetApprenticeSkills().GetUnlockedSkills());
    }

    public void DeselectApprentice() {

        selectedApprentice.GetComponent<Renderer>().material.color = Color.green;
        selectedApprentice = null;
        uiSkillTree.SetVisible(false);
    }
}
