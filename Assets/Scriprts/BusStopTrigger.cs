using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopTrigger : MonoBehaviour
{
    public GameObject prefabToSpawn;  // ������, ������� ����� ��������
    public bool isAtBusStop = false; // ���������� ��� ������������, ��������� �� ������� �� ���������
    public Vector3 spawnOffset = Vector3.zero; // �������� ���� ������ 
    [SerializeField] public int indexStop; // ������ ���� ���������
    int count = 0;
    private FindWay findWay;
    [SerializeField] private int bab;
    [SerializeField] public bool lastStop = false;


    public Vector2 spawnAreaSize = new Vector2(5f, 3f); // ������� ���� ������ 
    public float minDistanceBetweenPassengers = 1f; // ����������� ���������� ����� �����������
    public float baseRotationY = 0f; // ������� ���� �������� 

    private List<Vector3> spawnedPositions = new List<Vector3>(); // ���������� ������� ��� ������������ ����������

    private void Start()
    {
        findWay = FindObjectOfType<FindWay>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = true;
            Debug.Log("������� ������ �� ��������� � ��������: " + indexStop);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = false;
            Debug.Log("������� ������� ��������� � ��������: " + indexStop);
            findWay.ICanMoveAll();
        }
    }
    private void LateUpdate()
    {

        while (count < bab)
        {
            SpawnPrefab();
            count++;
        }
    }
    public void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            Vector3 spawnPosition;
            bool validPosition = false;
            int attempts = 10; // ���������� ������� ����� ���������� �������

            while (!validPosition && attempts > 0)
            {
                // ���������� ��������� ������� � �������� ������������� ����
                float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
                spawnPosition = transform.position + spawnOffset + new Vector3(randomX, 0, randomZ);

                // ���������, ����� ����� ����� �� ���� ������� ������ � ������ ����������
                validPosition = true;
                foreach (Vector3 pos in spawnedPositions)
                {
                    if (Vector3.Distance(pos, spawnPosition) < minDistanceBetweenPassengers)
                    {
                        validPosition = false;
                        break;
                    }
                }

                attempts--;
                if (validPosition)
                {
                    spawnedPositions.Add(spawnPosition);

                    // ������� ������
                    GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                    // ��������� ���� ��������
                    float randomRotationY = Random.Range(0f, 360f);
                    spawnedObject.transform.rotation = Quaternion.Euler(0, baseRotationY + randomRotationY, 0);
                    WayTest prefabScript = spawnedObject.GetComponent<WayTest>();
                    if (prefabScript != null)
                    {
                        prefabScript.SetIndex(indexStop, lastStop);
                    }
                }
            }
        }
    }

    // ������ ����� � ����� ��� ����������� ���� ������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + spawnOffset;
        Gizmos.DrawWireCube(center, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
    }
}