using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorController : MonoBehaviour
{
    // Ссылка на компонент Animator
    private Animator animator;
    // Имя триггера для открытия дверей
    private const string OpenDoorsTrigger = "OpenDoors";
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator не найден! Добавьте компонент Animator к двери.");
        }
        ButtonDoor.OnButtonPressed += ToggleDoor;
    }
    public void ToggleDoor()
    {        
        animator.SetTrigger(OpenDoorsTrigger); // Активируем триггер для анимации
    }
}