using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopTrigger : MonoBehaviour
{
    public GameObject prefabToSpawn;  // ������, ������� ����� ��������
    public bool isAtBusStop = false; // ���������� ��� ������������, ��������� �� ������� �� ���������
    [SerializeField] public int indexStop; // ������ ���� ���������
    int count = 0;
    [SerializeField] private int bab;

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
            PassengerMove prefabScript = spawnedObject.GetComponent<PassengerMove>();
            if (prefabScript != null)
            {
                prefabScript.SetIndex(indexStop);
            }
        }
    }
}