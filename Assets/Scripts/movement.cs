using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    public Transform cameraTransform; // Reference to the camera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate input direction relative to camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten the directions (ignore y to prevent flying/tilting)
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Calculate the final movement direction
        Vector3 moveDirection = camForward * verticalInput + camRight * horizontalInput;

        // Move the cylinder using Rigidbody
        Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}
