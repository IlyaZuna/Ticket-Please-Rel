using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusInteraction : MonoBehaviour
{
    public GameObject player;       // Персонаж, которого перемещаем в автобус
    public GameObject bus;          // Автобус, который будем контролировать
    public GameObject busCamera;    // Камера автобуса
    public GameObject playerCamera; // Камера персонажа

    private bool isNearBus = false;
    private bool isInBus = false;
    public Transform childObject;
    public Transform parentObject;

    private void Start()
    {
        // Убедимся, что при запуске управления автобусом и его камера отключены
        bus.GetComponent<BusController>().enabled = false;
        busCamera.SetActive(false);

        // Включаем персонажа и его камеру
        player.SetActive(true);
        playerCamera.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в зону триггера вошел персонаж
        if (other.gameObject == player)
        {
            isNearBus = true;
            Debug.Log("Press F to enter the bus.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Когда персонаж выходит из зоны триггера
        if (other.gameObject == player)
        {
            isNearBus = false;
        }
    }

    private void Update()
    {
        // Если рядом с автобусом и нажата F, а персонаж не в автобусе
        if (isNearBus && Input.GetKeyDown(KeyCode.F) && !isInBus)
        {
            EnterBus();
        }
        // Если персонаж уже в автобусе и нажата F, чтобы выйти
        else if (isInBus && Input.GetKeyDown(KeyCode.F))
        {
            ExitBus();
        }
    }

    private void EnterBus()
    {
        // Отключаем персонажа и его камеру
        childObject.SetParent(parentObject);
        player.SetActive(false);
        playerCamera.SetActive(false);

        // Включаем автобус и камеру автобуса
        bus.GetComponent<BusController>().enabled = true;
        busCamera.SetActive(true);

        isInBus = true;
        Debug.Log("Press F to exit the bus.");
    }

    private void ExitBus()
    {
        // Включаем персонажа и его      
        childObject.SetParent(null);
        player.SetActive(true);
        playerCamera.SetActive(true);

        // Отключаем автобус и камеру автобуса
        bus.GetComponent<BusController>().enabled = false;
        busCamera.SetActive(false);

        isInBus = false;
        Debug.Log("Press F to enter the bus.");
    }

    
}
