using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform; // Assign this in inspector or automatically find it
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movementInput;

    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (!canMove)
        {
            movementInput = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get camera's forward and right vectors, ignoring Y axis
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Movement relative to camera direction
        movementInput = camForward * moveVertical + camRight * moveHorizontal;

        float speed = movementInput.magnitude;
        animator.SetFloat("Speed", speed);

        // Rotate player toward movement direction if moving
        if (movementInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}
