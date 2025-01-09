using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopTrigger : MonoBehaviour
{
    public bool isAtBusStop = false; // Переменная для отслеживания, находится ли автобус на остановке
    [SerializeField] public int indexStop; // Индекс этой остановки

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
}