using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveCars : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 120f;
    public float stoppingDistance = 0.5f;
    public float pathUpdateTolerance = 1.0f;
    public float acceleration = 8f;
    [Range(1, 99)] public int avoidancePriority = 50;

    [Header("Stuck Detection")]
    public float stuckTimeThreshold = 2f;
    public float minMoveDistance = 0.5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float originalSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = moveSpeed;
        ConfigureAgent();

        if (waypoints.Length > 0)
        {
            SetNextDestination();
            lastPosition = transform.position;
        }
        else
        {
            Debug.LogError("No waypoints assigned!", this);
        }
    }

    void ConfigureAgent()
    {
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;
        agent.acceleration = acceleration;
        agent.avoidancePriority = avoidancePriority;
        agent.autoRepath = true;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        CheckMovementProgress();
        CheckIfStuck();
        AdjustSpeedBasedOnDistance();
    }

    void CheckMovementProgress()
    {
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance + pathUpdateTolerance)
        {
            SetNextDestination();
        }
    }

    void CheckIfStuck()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved < minMoveDistance)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckTimeThreshold)
            {
                HandleStuckSituation();
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    void HandleStuckSituation()
    {
        Debug.Log($"{gameObject.name} is stuck, attempting to recover", this);

        // 1. ѕопробуем пересчитать путь к текущей точке
        agent.ResetPath();
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // 2. ≈сли через 0.5 сек все еще застр€ли, выберем следующую точку
        Invoke("ForceNextWaypointIfStillStuck", 0.5f);

        stuckTimer = 0f;
    }

    void ForceNextWaypointIfStillStuck()
    {
        if (Vector3.Distance(transform.position, lastPosition) < minMoveDistance)
        {
            SetNextDestination();
        }
    }

    void AdjustSpeedBasedOnDistance()
    {
        if (agent.remainingDistance < 5f)
        {
            // ѕлавное уменьшение скорости при приближении к точке
            agent.speed = Mathf.Lerp(originalSpeed * 0.3f, originalSpeed, agent.remainingDistance / 5f);
        }
        else
        {
            agent.speed = originalSpeed;
        }
    }

    void SetNextDestination()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        // ѕровер€ем, доступна ли точка на NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(waypoints[currentWaypointIndex].position, out hit, 2.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning($"Waypoint {currentWaypointIndex} is not reachable, skipping", this);
            // ѕропускаем недоступную точку
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            int nextIndex = (i + 1) % waypoints.Length;
            if (waypoints[nextIndex] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
                DrawArrow(waypoints[i].position, waypoints[nextIndex].position);
            }

            Gizmos.DrawSphere(waypoints[i].position, 0.3f);
        }
    }

    void DrawArrow(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        float arrowHeadLength = 0.5f;
        float arrowHeadAngle = 20f;

        Vector3 right = Quaternion.LookRotation(direction) *
                       Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                       Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) *
                      Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                      Vector3.forward;

        Gizmos.DrawRay(to, right * arrowHeadLength);
        Gizmos.DrawRay(to, left * arrowHeadLength);
    }
}