using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class DisplayValues : MonoBehaviour
{
    [Header("Ссылки на скрипты")]
    [SerializeField] private ManagerStats scriptB;
    [SerializeField] private DriverIncome scriptA;

    [Header("UI элементы")]
    [SerializeField] private TMP_Text textA;
    [SerializeField] private TMP_Text textB;
    [SerializeField] private GameObject statsPanel; // Ссылка на панель с показателями

    [Header("Настройки")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool updateEveryFrame = true;

    private void Start()
    {
        // Проверка ссылок
        if (scriptA == null) Debug.LogError("ScriptA (DriverIncome) не назначен!");
        if (scriptB == null) Debug.LogError("ScriptB (ManagerStats) не назначен!");
        if (textA == null || textB == null) Debug.LogError("Текстовые поля не назначены!");
        if (statsPanel == null) Debug.LogError("Панель статистики не назначена!");

        // Скрываем панель при старте
        statsPanel.SetActive(false);
    }

    private void Update()
    {
        // Переключение панели по нажатию Tab
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }

        // Обновление значений
        if (updateEveryFrame && statsPanel.activeSelf)
        {
            UpdateValues();
        }
    }

    private void TogglePanel()
    {
        bool newState = !statsPanel.activeSelf;
        statsPanel.SetActive(newState);

        // Обновляем значения при открытии
        if (newState)
        {
            UpdateValues();
        }
    }

    private void UpdateValues()
    {
        if (scriptB != null) textA.text = scriptB.getPasengerSkore().ToString();
        if (scriptA != null) textB.text = scriptA.GetIncam().ToString();
    }
}