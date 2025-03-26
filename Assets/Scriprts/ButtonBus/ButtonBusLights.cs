using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBusLights : MonoBehaviour, IInteractable
{
    [Tooltip("Перетащите сюда родительский объект со всеми лампами салона")]
    [SerializeField] private GameObject lightsParent; // Родитель всех ламп

    [Tooltip("Начальное состояние света (выключен по умолчанию)")]
    [SerializeField] private bool startDisabled = true;

    private void Start()
    {
        // Выключаем свет при старте, если нужно
        if (startDisabled && lightsParent != null)
        {
            lightsParent.SetActive(false);
        }
    }

    public void Interact()
    {
        if (lightsParent != null)
        {
            // Переключаем состояние (вкл/выкл)
            lightsParent.SetActive(!lightsParent.activeSelf);

            // Можно добавить звук нажатия или другие эффекты
            Debug.Log("Свет салона: " + (lightsParent.activeSelf ? "ВКЛ" : "ВЫКЛ"));
        }
        else
        {
            Debug.LogError("Не назначен объект с лампами!", this);
        }
    }
}