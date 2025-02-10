using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopTrigger : MonoBehaviour
{
    public GameObject prefabToSpawn;  // Префаб, который будем спавнить
    public bool isAtBusStop = false; // Переменная для отслеживания, находится ли автобус на остановке
    [SerializeField] public int indexStop; // Индекс этой остановки
    int count = 0;
    [SerializeField] private int bab;

    // Когда автобус заезжает в зону триггера
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = true;
            Debug.Log("Автобус прибыл на остановку с индексом: " + indexStop);
        }
    }

    // Когда автобус покидает зону триггера
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = false;
            Debug.Log("Автобус покинул остановку с индексом: " + indexStop);
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

            // Передача индекса в скрипт префаба
            PassengerMove prefabScript = spawnedObject.GetComponent<PassengerMove>();
            if (prefabScript != null)
            {
                prefabScript.SetIndex(indexStop);
            }
        }
    }
}