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
    private float maxZoom = 80f;

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
    private Coroutine currentMoveCoroutine;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        Vector3 movement = new Vector3(moveValue.x, 0f, moveValue.y) * speed * Time.deltaTime;

        Vector3 targetPosition = transform.position + movement;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = 22.5f; 
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ); 

        transform.position = targetPosition;

        HandleZoom();
        HandleRotation();
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
        float temp = moveValue.x;
        moveValue.x = -moveValue.y;
        moveValue.y = temp;
    }

    void HandleZoom() {
        float scrollValue = Mouse.current.scroll.ReadValue().y;

        float currentZoom = cam.fieldOfView;
        currentZoom -= scrollValue * zoomSpeed * Time.deltaTime * 100;

        cam.fieldOfView = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void HandleRotation() {
        if (Mouse.current.rightButton.isPressed) {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * yawSpeed * Time.deltaTime;
            pitch -= mouseDelta.y * pitchSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }

    // move camera smoothly toward the apprentice
    public void MoveToApprentice(Vector3 apprenticePosition, float duration = 1f) {

        if (currentMoveCoroutine != null) {
            StopCoroutine(currentMoveCoroutine);
        }
        currentMoveCoroutine = StartCoroutine(MoveToApprenticeCoroutine(apprenticePosition, duration));
    }


    private IEnumerator MoveToApprenticeCoroutine(Vector3 apprenticePosition, float duration) {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 finalCameraPos = apprenticePosition + new Vector3(10f, 22.5f - apprenticePosition.y, 0f);

        Vector3 flatDirection = new Vector3(
            apprenticePosition.x - finalCameraPos.x,
            0f,
            apprenticePosition.z - finalCameraPos.z
        );
        if (flatDirection.sqrMagnitude < 0.001f) {
            flatDirection = Vector3.forward;
        }

        // angle is set to 60 to look down at apprentice
        Quaternion baseRotation = Quaternion.LookRotation(flatDirection.normalized, Vector3.up);
        Vector3 euler = baseRotation.eulerAngles;
        euler.x = 60f;
        Quaternion targetRot = Quaternion.Euler(euler);

        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 lerpedPosition = Vector3.Lerp(startPosition, finalCameraPos, t);
            // clamp each step
            lerpedPosition = ClampPosition(lerpedPosition);

            Quaternion lerpedRotation = Quaternion.Slerp(startRotation, targetRot, t);

            transform.position = lerpedPosition;
            transform.rotation = lerpedRotation;

            yield return null;
        }

        // final clamp
        transform.position = ClampPosition(finalCameraPos);
        transform.rotation = targetRot;
        currentMoveCoroutine = null;
    }


    private Vector3 ClampPosition(Vector3 pos) {

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        pos.y = 22.5f;
        return pos;
    }

}