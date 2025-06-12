using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;      // The cylinder to follow
    public Vector3 offset;        // Offset from the target
    public float rotationSpeed = 5f; // How fast the camera rotates

    private float currentYaw = 0f;
    private float currentPitch = 20f; // Optional initial pitch
    public float minPitch = -30f;
    public float maxPitch = 60f;

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Update rotation angles
        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // Calculate the new position & rotation
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Set camera position and look at the target
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
