using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SpawnApprentice : MonoBehaviour { 

    public GameObject apprentice; // the apprentice prefab to instantiate
    public GameObject attackArea; // the attack area prefab
    private Camera cam;

    private int apprenticeCount;
    private const int maxApprentices = 5; // max apprentices is immutable
    public TextMeshProUGUI apprenticeText;

    [SerializeField] private Transform apprentices;
    private GameController gameController;

    [SerializeField] private UISkillTree uiSkillTree;

    void Start() {

        cam = Camera.main;
        apprenticeCount = 0;
        apprenticeText.text = "";
        SetApprenticeText();
        gameController = FindAnyObjectByType<GameController>();
    }

    void FixedUpdate() {

        // update apprentice count in UI
        SetApprenticeText();
    }

    // decrease apprentice count and hide skill tree on death
    public void killApprentice() {

        apprenticeCount--;
        uiSkillTree.SetVisible(false);

    }

    void OnSpawn() { 

        // spawn an apprentice only if within maxApprentices 
        if (apprenticeCount < maxApprentices && Mouse.current.leftButton.wasPressedThisFrame) { 
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (gameController.selectedApprentice == null)
                {
                    Vector3 spawnPosition = new Vector3(hit.point.x, 0.5f, hit.point.z);

                    // check first if the attempted spawn position is within bounds of the floor
                    if (spawnPosition.x >= -9f && spawnPosition.x <= 9f && spawnPosition.z >= -9f && spawnPosition.z <= 9f && spawnPosition.y == 0.5)
                    {
                        // instantiate the apprentice prefab
                        GameObject newApprentice = Instantiate(apprentice, spawnPosition, Quaternion.identity);
                        newApprentice.transform.SetParent(apprentices);

                        newApprentice.name = "Apprentice " + (apprenticeCount + 1);
                        apprenticeCount++;

                        // instantiate the apprentice's attack area and set it as its child
                        GameObject newAttackArea = Instantiate(attackArea, newApprentice.transform.position, Quaternion.identity);
                        newAttackArea.transform.SetParent(newApprentice.transform);

                        ApprenticeController apprenticeController = newApprentice.GetComponent<ApprenticeController>();

                        Debug.Log("New apprentice spawned with skills system");
                    }
                }
            }
        }
    }


    // update apprentice count in UI
    private void SetApprenticeText() {

        apprenticeText.text = "Apprentices: " + apprenticeCount.ToString() + "/" + maxApprentices;
    }
}
