using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopTrigger : MonoBehaviour
{
    public bool isAtBusStop = false; // Переменная для отслеживания, находится ли автобус на остановке

    private void Start()
    {
        isAtBusStop = true;
    }
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = true;
            Debug.Log("Автобус прибыл на остановку.");
        }
    }
    private void OnTriggerExit(Collider other)
    {      
        if (other.CompareTag("Bus"))
        {
            isAtBusStop = false;
            Debug.Log("Автобус покинул остановку.");
        }
    }
}