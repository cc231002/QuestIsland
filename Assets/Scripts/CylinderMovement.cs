using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // how fast the player moves
    public float rotationSpeed = 10f; // how fast the player turns
    public Transform cameraTransform; // camera ref to get movement direction
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movementInput; // direction to move

    public bool canMove = true; // used to freeze/unfreeze player

    private bool isWalking = false; // NEW: to track sound state

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // grab rigidbody component
        animator = GetComponentInChildren<Animator>(); // get animator on child (probably the model)

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // fallback if not set manually
        }

        if (animator != null)
        {
            animator.SetBool("pressButton", false); // make sure animation starts clean
        }
    }

    void Update()
    {
        if (!canMove)
        {
            // player is frozen (maybe hit or in cutscene)
            movementInput = Vector3.zero;
            animator.SetFloat("Speed", 0f); // stop walk animation
            StopWalkingSound(); // stop if frozen
            return;
        }

        // get input from keys/joystick
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // get forward and right from camera so player moves in cam direction
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // figure out direction to move
        movementInput = camForward * moveVertical + camRight * moveHorizontal;

        float speed = movementInput.magnitude; // how much input player gave
        animator.SetFloat("Speed", speed); // pass speed to animator

        // Walking Sound Logic
        if (speed > 0.1f && !isWalking)
        {
            // just started walking
            AkUnitySoundEngine.PostEvent("Play_Walk", gameObject);
            isWalking = true;
        }
        else if (speed <= 0.1f && isWalking)
        {
            // stopped walking
            StopWalkingSound();
        }

        if (movementInput != Vector3.zero)
        {
            // smoothly rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // apply actual movement with physics
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    void StopWalkingSound()
    {
        // call Wwise to stop walk sound
        AkUnitySoundEngine.PostEvent("Stop_Walk", gameObject);
        isWalking = false;
    }
}
