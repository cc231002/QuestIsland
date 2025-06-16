using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Make sure Animator is on child!
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(moveHorizontal, 0.0f, moveVertical);

        float speed = movementInput.magnitude;
        animator.SetFloat("Speed", speed);

        Debug.Log("✅ Speed parameter sent to Animator: " + speed);
        Debug.Log("🎮 Input H: " + moveHorizontal + ", V: " + moveVertical);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        Debug.Log("🚀 Rigidbody velocity approx: " + movementInput.magnitude * moveSpeed);
    }
}
