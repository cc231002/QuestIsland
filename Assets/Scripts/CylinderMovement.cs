using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    public Transform cameraTransform;

    private bool isWalking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * verticalInput + camRight * horizontalInput;

        Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // --- Walking sound logic ---
        bool isMoving = moveDirection.magnitude > 0.1f;

        if (isMoving && !isWalking)
        {
            AkUnitySoundEngine.PostEvent("Play_Walk", gameObject); // Start walking sound (looping)
            isWalking = true;
        }
        else if (!isMoving && isWalking)
        {
            AkUnitySoundEngine.PostEvent("Stop_Walk", gameObject); // Stop walking sound
            isWalking = false;
        }
    }
}
