using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public InputActionAsset playerControls;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction zoomAction;

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

    private void Awake()
    {
        cam = Camera.main;
        InputActionMap playerActionMap = playerControls.FindActionMap("Camera");

        if (playerActionMap != null)
        {
            moveAction = playerActionMap.FindAction("Move");
            lookAction = playerActionMap.FindAction("Look");
            zoomAction = playerActionMap.FindAction("Scroll");
        }
        else
        {
            Debug.LogError("Could not find Action Map 'Camera'. Check your Input Actions Asset.");
            return;
        }
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.Enable();
        if (lookAction != null) lookAction.Enable();
        if (zoomAction != null) zoomAction.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.Disable();
        if (lookAction != null) lookAction.Disable();
        if (zoomAction != null) zoomAction.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation(); 
    }

    void HandleMovement()
    {
        if (moveAction == null) return;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

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
        targetPosition.y = 22.5f; // Keep the y position fixed
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = targetPosition;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookValue = context.ReadValue<Vector2>();
        // ... your look logic
    }

    public void OnScroll(InputValue value)
    {
        float scrollValue = value.Get<Vector2>().y;
        Debug.Log("Scroll Value: " + scrollValue);

        float currentZoom = cam.fieldOfView;
        currentZoom -= scrollValue * zoomSpeed * Time.deltaTime * 100;

        cam.fieldOfView = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        if (lookAction == null) return;

        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * yawSpeed * Time.deltaTime;
            pitch -= mouseDelta.y * pitchSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
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
        Quaternion startRotation = transform.rotation;

        Vector3 finalCameraPos = apprenticePosition + new Vector3(10f, 22.5f - apprenticePosition.y, 0f);

        Vector3 flatDirection = new Vector3(
            apprenticePosition.x - finalCameraPos.x,
            0f,
            apprenticePosition.z - finalCameraPos.z
        );
        if (flatDirection.sqrMagnitude < 0.001f)
        {
            flatDirection = Vector3.forward;
        }

        // angle is set to 60 to look down at apprentice
        Quaternion baseRotation = Quaternion.LookRotation(flatDirection.normalized, Vector3.up);
        Vector3 euler = baseRotation.eulerAngles;
        euler.x = 60f;
        Quaternion targetRot = Quaternion.Euler(euler);

        float elapsed = 0f;
        while (elapsed < duration)
        {
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

    private Vector3 ClampPosition(Vector3 pos)
    {

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        pos.y = 22.5f;
        return pos;
    }
}