using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector2 moveValue;
    private float speed = 5f;
    private float zoomSpeed = 2f;
    private float minZoom = 20f;
    private float maxZoom = 80f;

    // Define the min and max corners for camera movement bounds
    private float minX = -5f; // Minimum X boundary
    private float maxX = 60f;  // Maximum X boundary
    private float minZ = -15f; // Minimum Z boundary
    private float maxZ = 32.5f;  // Maximum Z boundary

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
        float temp = moveValue.x;
        moveValue.x = -moveValue.y;
        moveValue.y = temp;
    }

    void Update()
    {
        // Calculate the movement based on input
        Vector3 movement = new Vector3(moveValue.x, 0f, moveValue.y) * speed * Time.deltaTime;

        // Apply the movement to the camera's position
        Vector3 targetPosition = transform.position + movement;

        // Clamp the camera's position within the defined bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX); // Clamp X-axis
        targetPosition.y = 22.5f; // Fixed Y-axis (as you want it to stay at 0)
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ); // Clamp Z-axis

        // Apply the new clamped position to the camera
        transform.position = targetPosition;

        // Handle zooming
        HandleZoom();
    }

    void HandleZoom()
    {
        float currentZoom = cam.fieldOfView;

        if (Keyboard.current.equalsKey.isPressed) // For + key
        {
            currentZoom -= zoomSpeed * Time.deltaTime * 100;
        }

        if (Keyboard.current.minusKey.isPressed) // For - key
        {
            currentZoom += zoomSpeed * Time.deltaTime * 100;
        }

        // Clamp the zoom to prevent going out of bounds
        cam.fieldOfView = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }
}