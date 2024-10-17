using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnApprentice : MonoBehaviour
{
    public GameObject apprentice; // The prefab to instantiate
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SCRIPT RUN");
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnSpawn()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            Debug.Log("MOUSE CLICK");

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, 0.5f, hit.point.z);

                // Instantiate the apprentice prefab at the hit point
                Instantiate(apprentice, spawnPosition, Quaternion.identity);
                Debug.Log("INSTANTIATE");
            }
        }
    }
}
