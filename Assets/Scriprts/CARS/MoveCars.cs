using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CircularWaypointMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints; // Массив точек маршрута
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
        agent.autoBraking = false; // Отключаем автоторможение для плавного движения

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
        // Проверяем, достигли ли текущей точки
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            SetNextDestination();
        }
    }

    void SetNextDestination()
    {
        if (waypoints.Length == 0) return;

        // Устанавливаем следующую точку назначения
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Переходим к следующей точке (с зацикливанием)
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    // Визуализация маршрута в редакторе
    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            // Линия между точками
            int nextIndex = (i + 1) % waypoints.Length;
            if (waypoints[nextIndex] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
            }

            // Маркеры точек
            Gizmos.DrawSphere(waypoints[i].position, 0.3f);
        }
    }
}