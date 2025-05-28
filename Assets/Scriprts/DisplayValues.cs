using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayValues : MonoBehaviour
{
    [Header("Ссылки на скрипты")]
    [SerializeField] private DriverIncome scriptA;
    [SerializeField] private ManagerStats scriptB;
    
    [Header("UI элементы")]
    [SerializeField] private TMP_Text textPassengers; // Пассажиры
    [SerializeField] private TMP_Text textIncome;     // Доход
    [SerializeField] private TMP_Text textUKP;        // Уровень УКП
    [SerializeField] private GameObject statsPanel;   // Ссылка на панель с показателями

    [Header("Настройки")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool updateEveryFrame = true;

    private void Start()
    {
        // Проверка ссылок
        if (scriptA == null) Debug.LogError("DriverIncome не назначен!");
        if (scriptB == null) Debug.LogError("ManagerStats не назначен!");
        if (textPassengers == null || textIncome == null || textUKP == null) Debug.LogError("Текстовые поля не назначены!");
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
        if (scriptB != null)
        {
            textPassengers.text = $"Пассажиры: {scriptB.getPasengerSkore()}";
        }
        if (scriptA != null)
        {
            textIncome.text = $"Доход: {scriptA.GetIncam()} руб.";
        }
        if (scriptB != null)
        {
            textUKP.text = $"УКП: {scriptB.GetUKPLevel()}";
        }
    }
}