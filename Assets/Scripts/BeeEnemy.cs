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

    // Wwise sound tracking
    private uint flyingSoundId = 0;

    enum BeeState
    {
        Patrolling,
        ChasingPlayer,
        Attacking,
        ReturningToPatrol
    }

    void Start()
    {
        // check if important references are assigned in the Inspector
        if (pointA == null || pointB == null || player == null)
        {
            Debug.LogError("Please assign pointA, pointB, and the player in the Inspector.");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        targetPoint = pointB; // start moving toward pointB first
        basePosition = transform.position;

        // getting player movement script to disable during attack
        playerMovement = player.GetComponent<CylinderMovement>();

        // Start playing bee flying sound using Wwise
        flyingSoundId = AkUnitySoundEngine.PostEvent("Play_Bee", gameObject);
    }

    void Update()
    {
        // check what the bee should do based on current state
        switch (currentState)
        {
            case BeeState.Patrolling:
                Patrol();       // move between pointA and pointB
                DetectPlayer(); // check if player is near
                break;

            case BeeState.ChasingPlayer:
                ChasePlayer();  // follow the player
                break;

            case BeeState.Attacking:
                // nothing happens here because Attack() handles everything
                break;

            case BeeState.ReturningToPatrol:
                ReturnToPatrol(); // go back to patrolling path
                break;
        }

        ApplyFloating(); // make it float up and down
    }

    void Patrol()
    {
        // move toward the current patrol point (x/z only)
        Vector3 targetPos = new Vector3(targetPoint.position.x, basePosition.y, targetPoint.position.z);
        basePosition = Vector3.MoveTowards(basePosition, targetPos, speed * Time.deltaTime);

        RotateTowards(targetPos); // look in the direction it's moving

        // if reached target, switch to the other point
        if (Vector3.Distance(basePosition, targetPos) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }

        // apply floating Y movement
        transform.position = new Vector3(basePosition.x, basePosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, basePosition.z);
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // if player is close enough, start chasing
        if (distanceToPlayer <= detectionRange)
        {
            returnPosition = basePosition; // remember current position to return later
            currentState = BeeState.ChasingPlayer;
            chaseTimer = 0f;
        }
    }

    void ChasePlayer()
    {
        chaseTimer += Time.deltaTime;

        // get the center of the player and bee (if they have colliders)
        Vector3 playerCenter = player.GetComponent<Collider>() != null ? player.GetComponent<Collider>().bounds.center : player.position;
        Collider beeCollider = GetComponent<Collider>();
        Vector3 beeCenter = beeCollider != null ? beeCollider.bounds.center : transform.position;

        float distanceToPlayer = Vector3.Distance(beeCenter, playerCenter);

        if (distanceToPlayer > 0.7f)
        {
            // move toward player but only on x/z
            Vector3 targetPos = new Vector3(playerCenter.x, basePosition.y, playerCenter.z);
            basePosition = Vector3.MoveTowards(basePosition, targetPos, speed * 1.5f * Time.deltaTime);
            transform.position = new Vector3(basePosition.x, basePosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, basePosition.z);
            RotateTowards(targetPos);
        }

        if (distanceToPlayer <= 0.983f)
        {
            Attack(); // close enough to attack
        }
        else if (chaseTimer >= chaseDuration)
        {
            currentState = BeeState.ReturningToPatrol; // give up chasing
        }
    }

    void Attack()
    {
        if (playerMovement != null)
        {
            playerMovement.canMove = false; // stop the player
        }

        currentState = BeeState.Attacking;
        animator.SetTrigger("AttackTrigger"); // play animation
        HeartManager.Instance.LoseHeart();     // remove a heart
        Invoke("FinishAttack", 1.5f);          // wait before returning to patrol
    }

    void FinishAttack()
    {
        if (playerMovement != null)
        {
            playerMovement.canMove = true; // re-enable player movement
        }

        currentState = BeeState.ReturningToPatrol;
    }

    void ReturnToPatrol()
    {
        basePosition = Vector3.MoveTowards(basePosition, returnPosition, speed * Time.deltaTime);
        RotateTowards(returnPosition);

        // if back to original position, go back to patrolling
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
            Quaternion adjustedRot = lookRot * Quaternion.Euler(0f, 90f, 0f); // bee model faces right by default?
            transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRot, Time.deltaTime * 5f);
        }
    }

    void ApplyFloating()
    {
        // Already applied inside movement methods
    }

    void OnDisable()
    {
        // Stop sound when bee is disabled
        if (flyingSoundId != 0)
        {
            AkUnitySoundEngine.StopPlayingID(flyingSoundId);
            flyingSoundId = 0;
        }
    }
}
