using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 1.5f;

    [Header("Rotation")]
    public float rotationSpeed = 200f;
    public float mouseSensitivity = 0.05f;
    public float touchSensitivity = 0.1f;
    public float minVerticalAngle = -10f;
    public float maxVerticalAngle = 60f;

    [Header("Smoothing")]
    public float smoothSpeed = 10f;

    [Header("Collision")]
    public LayerMask collisionMask; // set to your walls/environment layer
    public float collisionRadius = 0.2f;

    private float currentYaw = 0f;
    private float currentPitch = 20f;
    private float targetYaw;
    private float targetPitch;

    // Mouse
    private Vector2 lastMousePosition;
    private bool isMouseDragging;

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Start()
    {
        targetYaw = currentYaw;
        targetPitch = currentPitch;
    }

    void LateUpdate()
    {
        HandleInput();

        // Smooth rotation
        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * smoothSpeed);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * smoothSpeed);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 desiredDirection = rotation * Vector3.back;

        // --- WALL COLLISION ---
        float actualDistance = GetCollisionDistance(desiredDirection);

        transform.position = player.position + desiredDirection * actualDistance;
        transform.LookAt(player.position + Vector3.up * 1f);
    }

    float GetCollisionDistance(Vector3 direction)
    {
        // Spherecast from player toward desired camera position
        if (Physics.SphereCast(
            player.position,
            collisionRadius,
            direction,
            out RaycastHit hit,
            distance,
            collisionMask))
        {
            // Hard stop just before the wall
            return Mathf.Max(hit.distance - collisionRadius, minDistance);
        }

        return distance;
    }

    void HandleInput()
    {
        float deltaX = 0f;
        float deltaY = 0f;

        // --- MOUSE: right side of screen only ---
        var mouse = UnityEngine.InputSystem.Mouse.current;
        if (mouse != null)
        {
            if (mouse.rightButton.wasPressedThisFrame)
            {
                Vector2 pressPos = mouse.position.ReadValue();
                if (IsRightSide(pressPos))
                {
                    lastMousePosition = pressPos;
                    isMouseDragging = true;
                }
            }
            if (mouse.rightButton.wasReleasedThisFrame)
                isMouseDragging = false;

            if (isMouseDragging && mouse.rightButton.isPressed)
            {
                Vector2 currentPos = mouse.position.ReadValue();
                Vector2 delta = currentPos - lastMousePosition;
                deltaX = delta.x * mouseSensitivity;
                deltaY = delta.y * mouseSensitivity;
                lastMousePosition = currentPos;
            }
        }

        // --- TOUCH: right side of screen only ---
        if (Touch.activeTouches.Count >= 1)
        {
            foreach (var touch in Touch.activeTouches)
            {
                // Only process touches that started on the right side
                if (IsRightSide(touch.screenPosition) &&
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    deltaX = touch.delta.x * touchSensitivity;
                    deltaY = touch.delta.y * touchSensitivity;
                    break; // only use one finger for camera
                }
            }
        }

        targetYaw += deltaX * rotationSpeed * Time.deltaTime;
        targetPitch -= deltaY * rotationSpeed * Time.deltaTime;
        targetPitch = Mathf.Clamp(targetPitch, minVerticalAngle, maxVerticalAngle);
    }

    bool IsRightSide(Vector2 screenPos)
    {
        return screenPos.x > Screen.width * 0.5f;
    }
}