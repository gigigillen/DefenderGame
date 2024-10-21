using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SpawnApprentice : MonoBehaviour { 

    public GameObject apprentice; // The prefab to instantiate
    public GameObject attackArea;
    private Camera cam;

    private int apprenticeCount;
    private const int maxApprentices = 5; // max apprentices is immutable
    public TextMeshProUGUI apprenticeText;

    [SerializeField] private Transform apprentices;
    private GameController gameController;

    [SerializeField] private UISkillTree uiSkillTree;

    // Start is called before the first frame update
    void Start() {

        cam = Camera.main;
        apprenticeCount = 0;
        apprenticeText.text = "";
        SetApprenticeText();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        SetApprenticeText();
    }

    public void killApprentice() {
        apprenticeCount--;
        uiSkillTree.SetVisible(false);

    }

    void OnSpawn() { 
    
        if (apprenticeCount < maxApprentices && Mouse.current.leftButton.wasPressedThisFrame) { 
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (gameController.selectedApprentice == null)
                {
                    Vector3 spawnPosition = new Vector3(hit.point.x, 0.5f, hit.point.z);
                    Debug.Log(spawnPosition);

                    if (spawnPosition.x >= -9f && spawnPosition.x <= 9f && spawnPosition.z >= -9f && spawnPosition.z <= 9f && spawnPosition.y == 0.5)
                    {
                        // Instantiate the apprentice prefab at the hit point
                        GameObject newApprentice = Instantiate(apprentice, spawnPosition, Quaternion.identity);
                        newApprentice.transform.SetParent(apprentices);

                        newApprentice.name = "Apprentice " + (apprenticeCount + 1);
                        apprenticeCount++;

                        GameObject newAttackArea = Instantiate(attackArea, newApprentice.transform.position, Quaternion.identity);
                        newAttackArea.transform.SetParent(newApprentice.transform);

                        ApprenticeController apprenticeController = newApprentice.GetComponent<ApprenticeController>();

                        Debug.Log("New apprentice spawned with skills system");
                    }
                }
            }
        }
    }

    private void SetApprenticeText() {

        apprenticeText.text = "Apprentices: " + apprenticeCount.ToString() + "/" + maxApprentices;
    }
}
