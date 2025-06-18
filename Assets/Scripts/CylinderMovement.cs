using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movementInput;

    public bool canMove = true;

    private bool isWalking = false; // NEW: to track sound state

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
            StopWalkingSound(); // stop if frozen
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        movementInput = camForward * moveVertical + camRight * moveHorizontal;

        float speed = movementInput.magnitude;
        animator.SetFloat("Speed", speed);

        // Walking Sound Logic
        if (speed > 0.1f && !isWalking)
        {
            AkUnitySoundEngine.PostEvent("Play_Walk", gameObject);
            isWalking = true;
        }
        else if (speed <= 0.1f && isWalking)
        {
            StopWalkingSound();
        }

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

    void StopWalkingSound()
    {
        AkUnitySoundEngine.PostEvent("Stop_Walk", gameObject);
        isWalking = false;
    }
}
