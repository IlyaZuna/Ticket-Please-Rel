using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorController : MonoBehaviour {
    // Ссылка на компонент Animator
    private Animator animator;

    // Имя триггера для открытия дверей
    private const string OpenDoorsTrigger = "OpenDoors";

    void Start() {
        // Получаем компонент Animator на объекте
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator не найден! Добавьте компонент Animator к объекту.");
        }
    }

    void Update() {
        // Проверяем нажатие клавиши K
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (animator != null)
            {
                // Активируем триггер для анимации
                animator.SetTrigger(OpenDoorsTrigger);
            }
        }
    }
}
