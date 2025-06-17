using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movementInput;

    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
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
        movementInput = new Vector3(moveHorizontal, 0.0f, moveVertical);

        float speed = movementInput.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}
