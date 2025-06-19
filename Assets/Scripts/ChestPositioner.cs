using UnityEngine;

public class ChestPositioner : MonoBehaviour
{
    [Header("Reference to the camera")]
    public Camera targetCamera; // Assign manually or leave empty to use main camera

    [Header("Position offset from the camera")]
    public Vector3 positionOffset = new Vector3(0, -0.5f, 2f); // Forward and slightly down

    [Header("Initial rotation of the chest (world rotation)")]
    public Vector3 initialRotation = new Vector3(0, 90, 0); // Rotate once to the right

    void Start()
    {
        // Use assigned camera, or default to main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera != null)
        {
            // Position the chest in front of the camera with offset
            transform.position = targetCamera.transform.position + targetCamera.transform.rotation * positionOffset;

            // Apply initial rotation (absolute world rotation)
            transform.rotation = Quaternion.Euler(initialRotation);
        }
        else
        {
            Debug.LogWarning("No camera found. Please assign a camera manually.");
        }
    }
}