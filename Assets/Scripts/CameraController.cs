using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector2 moveValue;
    private Vector2 rotationValue;
    private float speed = 10f;
    private float zoomSpeed = 0.5f;
    private float minZoom = 20f;
    private float maxZoom = 100f;

    private float minX = -25f;
    private float maxX = 40f;
    private float minZ = -15f;
    private float maxZ = 32.5f;

    private float pitch = 30f;
    private float yaw = 0f;
    private float pitchSpeed = 100f;
    private float yawSpeed = 100f;
    private float minPitch = 40f;
    private float maxPitch = 60f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        // Get the camera's forward and right directions
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Project forward and right directions onto the XZ plane (ignore Y)
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Calculate movement direction relative to camera
        Vector3 movement = (right * moveValue.x + forward * moveValue.y) * speed * Time.deltaTime;

        Vector3 targetPosition = transform.position + movement;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = 22.5f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = targetPosition;
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void HandleZoom()
    {
        float scrollValue = Mouse.current.scroll.ReadValue().y;

        float currentZoom = cam.fieldOfView;
        currentZoom -= scrollValue * zoomSpeed * Time.deltaTime * 100;

        cam.fieldOfView = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * yawSpeed * Time.deltaTime;
            pitch -= mouseDelta.y * pitchSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }
}