using UnityEngine;

public class BeeEnemy : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Floating Movement")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    [Header("Detection & Attack")]
    public float detectionRange = 1f;
    public float chaseDuration = 5f;
    public Transform player;

    private Animator animator;
    private Vector3 basePosition;
    private Vector3 returnPosition;
    private Transform targetPoint;
    private BeeState currentState = BeeState.Patrolling;
    private float chaseTimer = 0f;
    private CylinderMovement playerMovement;

    enum BeeState
    {
        Patrolling,
        ChasingPlayer,
        Attacking,
        ReturningToPatrol
    }

    void Start()
    {
        if (pointA == null || pointB == null || player == null)
        {
            Debug.LogError("Please assign pointA, pointB, and the player in the Inspector.");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        targetPoint = pointB;
        basePosition = transform.position;

        playerMovement = player.GetComponent<CylinderMovement>();

    }

    void Update()
    {
        switch (currentState)
        {
            case BeeState.Patrolling:
                Patrol();
                DetectPlayer();
                break;

            case BeeState.ChasingPlayer:
                ChasePlayer();
                break;

            case BeeState.Attacking:
                // Optional: Add visual or sound effects here
                break;

            case BeeState.ReturningToPatrol:
                ReturnToPatrol();
                break;
        }

        ApplyFloating(); // Floating logic is handled within movement functions
    }

    void Patrol()
    {
        Vector3 targetPos = new Vector3(targetPoint.position.x, basePosition.y, targetPoint.position.z);
        basePosition = Vector3.MoveTowards(basePosition, targetPos, speed * Time.deltaTime);

        RotateTowards(targetPos);

        if (Vector3.Distance(basePosition, targetPos) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }

        transform.position = new Vector3(basePosition.x, basePosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, basePosition.z);
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            returnPosition = basePosition;
            currentState = BeeState.ChasingPlayer;
            chaseTimer = 0f;
        }
    }

    void ChasePlayer()
    {
        chaseTimer += Time.deltaTime;

        Vector3 playerCenter = player.GetComponent<Collider>() != null ? player.GetComponent<Collider>().bounds.center : player.position;
        Collider beeCollider = GetComponent<Collider>();
        Vector3 beeCenter = beeCollider != null ? beeCollider.bounds.center : transform.position;

        float distanceToPlayer = Vector3.Distance(beeCenter, playerCenter);
        Debug.Log("Chasing player - Distance: " + distanceToPlayer + ", Time: " + chaseTimer);

        if (distanceToPlayer > 0.7f)
        {
            Vector3 targetPos = new Vector3(playerCenter.x, basePosition.y, playerCenter.z);
            basePosition = Vector3.MoveTowards(basePosition, targetPos, speed * 1.5f * Time.deltaTime);
            transform.position = new Vector3(basePosition.x, basePosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, basePosition.z);
            RotateTowards(targetPos);
        }

        if (distanceToPlayer <= 0.983f)
        {
            Debug.Log("Starting attack!");
            Attack();
        }
        else if (chaseTimer >= chaseDuration)
        {
            currentState = BeeState.ReturningToPatrol;
        }
    }

    void Attack()
    {
        if (playerMovement != null)
        {
            playerMovement.canMove = false; // Disable player movement
        }

        currentState = BeeState.Attacking;
        Debug.Log("Attack() called");

        animator.SetTrigger("AttackTrigger");
        Debug.Log("Player loses a life!"); // replace with actual damage logic
        HeartManager.Instance.LoseHeart();
        Invoke("FinishAttack", 1.5f); // attack duration
    }

    void FinishAttack()
    {
        if (playerMovement != null)
        {
            playerMovement.canMove = true; // Re-enable movement
        }

        currentState = BeeState.ReturningToPatrol;
    }

    void ReturnToPatrol()
    {
        basePosition = Vector3.MoveTowards(basePosition, returnPosition, speed * Time.deltaTime);

        RotateTowards(returnPosition);

        if (Vector3.Distance(basePosition, returnPosition) < 0.05f)
        {
            currentState = BeeState.Patrolling;
        }

        transform.position = new Vector3(basePosition.x, basePosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, basePosition.z);
    }

    void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = targetPos - basePosition;
        Vector3 lookDir = new Vector3(direction.x, 0f, direction.z);
        if (lookDir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            Quaternion adjustedRot = lookRot * Quaternion.Euler(0f, 90f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRot, Time.deltaTime * 5f);
        }
    }

    void ApplyFloating()
    {
        // Floating is applied within movement logic
    }

    // Optional: Gizmos for debugging
    // void OnDrawGizmos() { ... }


    // void OnDrawGizmos()
    // {
    //     if (pointA != null && pointB != null)
    //     {
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawSphere(pointA.position, 0.2f);
    //         Gizmos.DrawSphere(pointB.position, 0.2f);
    //         Gizmos.color = Color.cyan;
    //         Gizmos.DrawLine(pointA.position, pointB.position);
    //     }

    //     if (player != null)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawWireSphere(transform.position, detectionRange);

    //         Collider playerCollider = player.GetComponent<Collider>();
    //         if (playerCollider != null)
    //         {
    //             Vector3 center = playerCollider.bounds.center;
    //             Gizmos.color = Color.green;
    //             Gizmos.DrawWireSphere(center, 0.2f); // show center point
    //             Gizmos.color = new Color(1, 0.5f, 0f, 0.3f);
    //             Gizmos.DrawLine(transform.position, center);
    //         }
    //     }
    // }
}
