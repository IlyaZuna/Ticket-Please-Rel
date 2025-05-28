using UnityEngine;
using System.Collections.Generic;

public class CARSpawn : MonoBehaviour
{
    [System.Serializable]
    public class Route
    {
        public List<Transform> waypoints;
    }

    [Header("Spawn Settings")]
    public List<GameObject> vehiclePrefabs; // Список префабов машин для случайного выбора
    public List<Transform> spawnPoints; // Список точек спавна
    public List<Route> routes; // Список маршрутов (каждый маршрут - список точек)
    public int vehiclesToSpawn = 5; // Количество машин для спавна

    [Header("Spawn Method")]
    public bool randomPrefabSelection = true; // Выбирать префаб случайным образом
    public int selectedPrefabIndex = 0; // Индекс выбранного префаба, если randomPrefabSelection = false

    [Header("Movement Settings")]
    public float defaultMoveSpeed = 10f;
    public float defaultRotationSpeed = 120f;
    public float defaultStoppingDistance = 1.5f;

    [Header("Spawned Vehicles")]
    [SerializeField] private List<GameObject> spawnedVehicles = new List<GameObject>(); // Список созданных машин

    void Start()
    {
        SpawnVehicles();
    }

    void SpawnVehicles()
    {
        if (vehiclePrefabs == null || vehiclePrefabs.Count == 0)
        {
            Debug.LogError("No vehicle prefabs assigned!");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        if (routes.Count == 0)
        {
            Debug.LogError("No routes assigned!");
            return;
        }

        // Очищаем список перед созданием новых машин
        ClearAllVehicles();

        // Ограничиваем количество машин количеством точек спавна
        int actualVehiclesToSpawn = Mathf.Min(vehiclesToSpawn, spawnPoints.Count);

        for (int i = 0; i < actualVehiclesToSpawn; i++)
        {
            // Выбираем точку спавна (по кругу, если машин больше чем точек)
            Transform spawnPoint = spawnPoints[i % spawnPoints.Count];

            // Выбираем префаб
            GameObject prefabToSpawn;
            if (randomPrefabSelection)
            {
                prefabToSpawn = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];
            }
            else
            {
                prefabToSpawn = vehiclePrefabs[Mathf.Clamp(selectedPrefabIndex, 0, vehiclePrefabs.Count - 1)];
            }

            // Создаем машину
            GameObject vehicle = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            spawnedVehicles.Add(vehicle);

            // Настраиваем компонент движения
            MoveCars movement = vehicle.GetComponent<MoveCars>();
            if (movement != null)
            {
                // Выбираем маршрут (по кругу, если машин больше чем маршрутов)
                Route selectedRoute = routes[i % routes.Count];

                // Проверяем, что маршрут не пустой
                if (selectedRoute.waypoints.Count == 0)
                {
                    Debug.LogWarning($"Route {i % routes.Count} has no waypoints! Using first available route.");

                    // Ищем первый непустой маршрут
                    foreach (Route route in routes)
                    {
                        if (route.waypoints.Count > 0)
                        {
                            selectedRoute = route;
                            break;
                        }
                    }
                }

                // Назначаем маршрут
                movement.waypoints = selectedRoute.waypoints.ToArray();
                movement.moveSpeed = defaultMoveSpeed;
                movement.rotationSpeed = defaultRotationSpeed;
                movement.stoppingDistance = defaultStoppingDistance;
            }
            else
            {
                Debug.LogError("Vehicle prefab doesn't have CircularWaypointMovement component!");
            }
        }
    }

    // Очистка всех созданных машин
    public void ClearAllVehicles()
    {
        foreach (GameObject vehicle in spawnedVehicles)
        {
            if (vehicle != null)
            {
                Destroy(vehicle);
            }
        }
        spawnedVehicles.Clear();
    }

    // Получить список всех созданных машин
    public List<GameObject> GetAllVehicles()
    {
        return new List<GameObject>(spawnedVehicles);
    }

    // Перезапуск спавна машин
    public void RespawnVehicles()
    {
        ClearAllVehicles();
        SpawnVehicles();
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        // Рисуем точки спавна
        Gizmos.color = Color.blue;
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            }
        }

        // Рисуем маршруты
        for (int i = 0; i < routes.Count; i++)
        {
            Color routeColor = Color.HSVToRGB(i / (float)routes.Count, 1f, 1f);
            Gizmos.color = routeColor;

            if (routes[i].waypoints.Count < 2) continue;

            for (int j = 0; j < routes[i].waypoints.Count; j++)
            {
                if (routes[i].waypoints[j] == null) continue;

                // Линия между точками
                int nextIndex = (j + 1) % routes[i].waypoints.Count;
                if (routes[i].waypoints[nextIndex] != null)
                {
                    Gizmos.DrawLine(routes[i].waypoints[j].position, routes[i].waypoints[nextIndex].position);
                }

                // Маркеры точек
                Gizmos.DrawSphere(routes[i].waypoints[j].position, 0.3f);
            }
        }
    }
}