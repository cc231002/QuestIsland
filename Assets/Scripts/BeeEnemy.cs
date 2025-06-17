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
            Debug.LogError("Setze pointA, pointB und den Player im Inspector.");
            enabled = false;
            return;
        }

        Debug.Log("Spieler gefunden: " + player.name);

        animator = GetComponent<Animator>();
        targetPoint = pointB;
        basePosition = transform.position;
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
                // Optional: kannst hier Effekte einbauen
                break;

            case BeeState.ReturningToPatrol:
                ReturnToPatrol();
                break;
        }

        ApplyFloating();
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
        //Debug.Log("Entfernung zum Spieler: " + distanceToPlayer);

        if (distanceToPlayer <= detectionRange)
        {
            //Debug.Log("Spieler erkannt! Wechsel zu Chase-Mode.");
            returnPosition = basePosition;
            currentState = BeeState.ChasingPlayer;
            chaseTimer = 0f;
        }
    }

    void ChasePlayer()
{
    chaseTimer += Time.deltaTime;

    // Mittelpunkte der Collider holen
    Vector3 playerCenter = player.GetComponent<Collider>() != null ? player.GetComponent<Collider>().bounds.center : player.position;
    Collider beeCollider = GetComponent<Collider>();
    Vector3 beeCenter = beeCollider != null ? beeCollider.bounds.center : transform.position;

    float distanceToPlayer = Vector3.Distance(beeCenter, playerCenter);
    Debug.Log("Chasing Player - Abstand: " + distanceToPlayer + ", Zeit: " + chaseTimer);

    // Nur bewegen, wenn weiter als z.B. 0.7 entfernt
    if (distanceToPlayer > 0.7f)
    {
        Vector3 targetPos = new Vector3(playerCenter.x, basePosition.y, playerCenter.z);
        basePosition = Vector3.MoveTowards(basePosition, targetPos, speed * 1.5f * Time.deltaTime);
        transform.position = new Vector3(basePosition.x, basePosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, basePosition.z);
        RotateTowards(targetPos);
    }

    // Angriff auslösen
    if (distanceToPlayer <= 0.983f)
    {
        Debug.Log("Angriff starten!");
        Attack();
    }
    else if (chaseTimer >= chaseDuration)
    {
        currentState = BeeState.ReturningToPatrol;
    }
}




    void Attack()
    {
        currentState = BeeState.Attacking;
        Debug.Log("Attack() aufgerufen");

        animator.SetTrigger("AttackTrigger");
        Debug.Log("Spieler verliert ein Leben!"); // später ersetzt dein Kollege das mit echter Logik
        Invoke("FinishAttack", 1.5f); // z.B. 1 Sekunde dauert der Angriff // Annahme: Attack dauert 1 Sekunde
    }

    void FinishAttack()
    {
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
        // handled in other functions now
    }

    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }

        if (player != null)
        {
            // Standard Detection-Sphere
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            // Spieler-Collider-Mittelpunkt anzeigen
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Vector3 center = playerCollider.bounds.center;

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(center, 0.2f); // zeigt Mittelpunkt
                Gizmos.color = new Color(1, 0.5f, 0f, 0.3f);
                Gizmos.DrawLine(transform.position, center);
            }
        }
    }


}
