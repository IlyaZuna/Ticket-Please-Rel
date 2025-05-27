using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CircularWaypointMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints; // ������ ����� ��������
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 120f;
    public float stoppingDistance = 0.5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = false; // ��������� �������������� ��� �������� ��������

        if (waypoints.Length > 0)
        {
            SetNextDestination();
        }
        else
        {
            Debug.LogError("No waypoints assigned!");
        }
    }

    void Update()
    {
        // ���������, �������� �� ������� �����
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            SetNextDestination();
        }
    }

    void SetNextDestination()
    {
        if (waypoints.Length == 0) return;

        // ������������� ��������� ����� ����������
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // ��������� � ��������� ����� (� �������������)
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    // ������������ �������� � ���������
    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            // ����� ����� �������
            int nextIndex = (i + 1) % waypoints.Length;
            if (waypoints[nextIndex] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
            }

            // ������� �����
            Gizmos.DrawSphere(waypoints[i].position, 0.3f);
        }
    }
}