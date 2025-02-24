using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopTrigger : MonoBehaviour
{
    public GameObject prefabToSpawn;  // ������, ������� ����� ��������
    public bool isAtBusStop = false; // ���������� ��� ������������, ��������� �� ������� �� ���������
    [SerializeField] public int indexStop; // ������ ���� ���������
    int count = 0;
    private FindWay findWay;
    [SerializeField] private int bab;
    [SerializeField] public bool lastStop = false;

    private void Start()
    {
        findWay = FindObjectOfType<FindWay>();
    }
    // ����� ������� �������� � ���� ��������
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = true;
            Debug.Log("������� ������ �� ��������� � ��������: " + indexStop);
        }
    }

    // ����� ������� �������� ���� ��������
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
            GameObject spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            // �������� ������� � ������ �������
            WayTest prefabScript = spawnedObject.GetComponent<WayTest>();
            if (prefabScript != null)
            {               
                    prefabScript.SetIndex(indexStop, lastStop);               
            }
        }
    }
}