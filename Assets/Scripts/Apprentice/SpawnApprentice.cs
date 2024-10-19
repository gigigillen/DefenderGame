using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SpawnApprentice : MonoBehaviour { 

    public GameObject apprentice; // The prefab to instantiate
    private Camera cam;

    private int apprenticeCount;
    private const int maxApprentices = 5; // max apprentices is immutable
    public TextMeshProUGUI apprenticeText;

    // Start is called before the first frame update
    void Start() {

        cam = Camera.main;
        apprenticeCount = 0;
        apprenticeText.text = "";
        SetApprenticeText();
    }

    // Update is called once per frame
    void FixedUpdate() {

        SetApprenticeText();
    }

    void OnSpawn() { 
    
        if (apprenticeCount < maxApprentices && Mouse.current.leftButton.wasPressedThisFrame) { 
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) { 
                Vector3 spawnPosition = new Vector3(hit.point.x, 0.5f, hit.point.z);

                // Instantiate the apprentice prefab at the hit point
                GameObject newApprentice = Instantiate(apprentice, spawnPosition, Quaternion.identity);
                newApprentice.name = "Apprentice " + (apprenticeCount + 1);
                apprenticeCount++;

                ApprenticeController apprenticeController = newApprentice.GetComponent<ApprenticeController>();

                Debug.Log("New apprentice spawned with skills system");
            }
        }
    }

    private void SetApprenticeText() {

        apprenticeText.text = "Apprentices: " + apprenticeCount.ToString() + "/" + maxApprentices;
    }
}
