using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public InputActionAsset playerControls;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction zoomAction;
    private InputAction mouseRightAction;

    private float speed = 10f;
    private float zoomSpeed = 0.5f;
    private float minZoom = 20f;
    private float maxZoom = 65f;

    private float minX = -25f;
    private float maxX = 40f;
    private float minZ = -15f;
    private float maxZ = 32.5f;

    private float pitch = 30f;
    private float yaw = 0f;
    private float pitchSpeed = 100f;
    private float yawSpeed = 100f;
    private float minPitch = 30f;
    private float maxPitch = 85f;

    private Camera cam;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Coroutine currentMoveCoroutine;
    private bool isRotating;

    private void Awake()
    {
        cam = Camera.main;
        InputActionMap playerActionMap = playerControls.FindActionMap("Camera");

        if (playerActionMap != null)
        {
            moveAction = playerActionMap.FindAction("Move");
            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;

            lookAction = playerActionMap.FindAction("Look");
            lookAction.performed += OnLook;
            lookAction.canceled += OnLook;

            zoomAction = playerActionMap.FindAction("Scroll");
            zoomAction.performed += OnZoom;
        }
        else
        {
            Debug.LogError("Could not find Action Map 'Camera'. Check your Input Actions Asset.");
            return;
        }

        mouseRightAction = playerActionMap.FindAction("MouseRight");
        mouseRightAction.performed += ctx => isRotating = true;
        mouseRightAction.canceled += ctx => isRotating = false;
    }

    private void Start() {

        Vector3 currentRotation = transform.eulerAngles;
        pitch = currentRotation.x;
        yaw = currentRotation.y;
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.Enable();
        if (lookAction != null) lookAction.Enable();
        if (zoomAction != null) zoomAction.Enable();
        if (mouseRightAction != null) mouseRightAction.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.Disable();
        if (lookAction != null) lookAction.Disable();
        if (zoomAction != null) zoomAction.Disable();
        if (mouseRightAction != null) mouseRightAction.Disable();
    }

    private void OnDestroy() {

        if (moveAction != null) {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }
        if (lookAction != null) {
            lookAction.performed -= OnLook;
            lookAction.canceled -= OnLook;
        }
        if (zoomAction != null) {
            zoomAction.performed -= OnZoom;
        }
        if (mouseRightAction != null) {
            mouseRightAction.performed -= ctx => isRotating = true;
            mouseRightAction.canceled -= ctx => isRotating = false;
        }
    }

    void Update()
    {
        HandleMovement();
        if (isRotating) {
            HandleRotation();
        }
    }

    private void OnMove(InputAction.CallbackContext context) {

        moveInput = context.ReadValue<Vector2>();
    }


    private void OnLook(InputAction.CallbackContext context) {

        lookInput = context.ReadValue<Vector2>();
    }

    private void OnZoom(InputAction.CallbackContext context) {

        float scrollValue = context.ReadValue<Vector2>().y;
        float currentZoom = cam.fieldOfView;
        float newZoom = currentZoom - (scrollValue * zoomSpeed * Time.deltaTime * 100);
        if (ExceedsGameBoundary(newZoom)) {
            newZoom = Mathf.Min(newZoom, maxZoom);
        }
        cam.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom); ;
    }

    private bool ExceedsGameBoundary(float fieldOfView) {

        float cameraHeight = transform.position.y;
        float wallHeight = 10f;
        float heightDifference = cameraHeight - wallHeight;

        float distanceToWall = Mathf.Max(Mathf.Abs(minX), Mathf.Abs(maxX),
                                       Mathf.Abs(minZ), Mathf.Abs(maxZ));
        float angleToWallTop = Mathf.Atan2(heightDifference, distanceToWall) * Mathf.Rad2Deg;

        return (fieldOfView * 0.5f) > angleToWallTop;
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
        Vector3 movement = (right * moveInput.x + forward * moveInput.y) * speed * Time.deltaTime;
        Vector3 targetPosition = transform.position + movement;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = 22.5f; // Keep the y position fixed
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = targetPosition;
    }


    void HandleRotation()
    {
        yaw += lookInput.x * yawSpeed * Time.deltaTime;
        pitch -= lookInput.y * pitchSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }


    // move camera smoothly toward the apprentice
    public void MoveToApprentice(Vector3 apprenticePosition, float duration = 1f)
    {

        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }
        currentMoveCoroutine = StartCoroutine(MoveToApprenticeCoroutine(apprenticePosition, duration));
    }


    private IEnumerator MoveToApprenticeCoroutine(Vector3 apprenticePosition, float duration)
    {
        Vector3 startPosition = transform.position;

        Vector3 finalCameraPos = apprenticePosition + new Vector3(15f, 22.5f - apprenticePosition.y, 0f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 lerpedPosition = Vector3.Lerp(startPosition, finalCameraPos, t);
            // clamp each step
            transform.position = ClampPosition(lerpedPosition);
            yield return null;
        }

        // final clamp
        transform.position = ClampPosition(finalCameraPos);
        currentMoveCoroutine = null;
    }

    private Vector3 ClampPosition(Vector3 pos)
    {

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        pos.y = 22.5f;
        return pos;
    }
}