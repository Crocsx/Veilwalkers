using UnityEngine;

public class PlayerHeadController : MonoBehaviour
{
    [SerializeField] private VirtualController virtualController;

    [SerializeField] private float baseSensitivityX = 25f;
    [SerializeField] private float baseSensitivityY = 50f;
    [SerializeField] private float deadZone = 0.05f; // Minimum input to register

    [SerializeField] private bool clampVerticalRotation = true;
    [SerializeField] private float maxVerticalAngle = 80f;
    [SerializeField] private float minVerticalAngle = -80f;
    [SerializeField] private bool clampHorizontalRotation = false;
    [SerializeField] private float maxHorizontalAngle = 360f;
    [SerializeField] private float minHorizontalAngle = -360f;
    [SerializeField] private float smoothing = 0f; // Adjust smoothing effect

    private Transform head;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Quaternion smoothRotation;

    void Awake()
    {
        InitializeCursor();
        head = transform;
        smoothRotation = head.localRotation;
    }

    void Update()
    {
        ProcessInput(Time.deltaTime);
        ApplyRotation(smoothing);
    }

    void InitializeCursor()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void ProcessInput(float deltaTime)
    {
        Vector2 lookInput = new Vector2(virtualController.LookAxisRight, virtualController.LookAxisUp);

        // Apply dead zone
        lookInput.x = Mathf.Abs(lookInput.x) < deadZone ? 0 : lookInput.x;
        lookInput.y = Mathf.Abs(lookInput.y) < deadZone ? 0 : lookInput.y;

        xRotation += lookInput.y * baseSensitivityY * deltaTime;
        yRotation += lookInput.x * baseSensitivityX * deltaTime;

        // Clamp rotations
        if (clampVerticalRotation) xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        if (clampHorizontalRotation) yRotation = Mathf.Clamp(yRotation, minHorizontalAngle, maxHorizontalAngle);
    }

    void ApplyRotation(float smoothingFactor)
    {
        // Calculate the target rotation based on the input rotations
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    
        // Smoothly interpolate towards the target rotation using the smoothing factor
        // The smoothing factor controls how quickly the rotation interpolates to the target value
        // A higher smoothing factor results in a faster interpolation, while a lower factor results in smoother, slower motion
        if (smoothingFactor > 0f)
        {
            smoothRotation = Quaternion.Lerp(smoothRotation, targetRotation, smoothingFactor * Time.deltaTime);
        }
        else
        {
            smoothRotation = targetRotation;
        }

        // Apply the smoothed rotation to the head transform
        head.localRotation = smoothRotation;
    }
}