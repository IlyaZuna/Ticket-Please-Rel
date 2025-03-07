using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StationManagerInteraction : MonoBehaviour
{
    public GameObject interactionPrompt; // Подсказка "E - взаимодействовать"
    public GameObject arrowIndicator; // Светящаяся стрелка над начальником
    public GameObject dialoguePanel; // Панель для субтитров
    public TMP_Text subtitleText; // Текст субтитров
    public GameObject taskPanel; // Панель с задачами
    private bool isPlayerInRange = false; // Проверяет, находится ли игрок рядом
    private bool isDialogueActive = false; // Активен ли диалог
    private bool hasCompletedFirstDialogue = false; // Завершён ли первый диалог полностью
    private int dialogueStep = 0; // Текущий шаг диалога
    private bool hasShownTasks = false; // Показаны ли задачи

    // Реплики начальника
    private string[] dialogueLines = new string[]
    {
        "Водитель, в последнее время поступает много жалоб от пассажиров...",
        "Министерство транспорта ввело новую тестовую систему контроля удовлетворённости пассажиров (СКУП).",
        "Теперь ты должен следить за уровнем комфорта пассажиров (УКП) и держать его максимальным!",
        "Каждая автостанция отчитывается о УКП в конце месяца. Лучшие водители получат премии и грамоты!",
        "Это отличный шанс накопить на новый автобус и начать работать на себя!"
    };

    // Задачи
    private string[] taskLines = new string[]
    {
        "Задачи на смену:",
        "- Перевести 20 пассажиров",
        "- Уровень УКП > 50%"
    };

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (arrowIndicator != null) arrowIndicator.SetActive(true); // Стрелка активна с начала
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (taskPanel != null) taskPanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && !isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (!hasCompletedFirstDialogue)
            {
                StartDialogue();
            }
            else if (hasShownTasks)
            {
                ShowTasks(); // Показываем задачи, если они уже были показаны
                if (interactionPrompt != null) interactionPrompt.SetActive(false); // Скрываем "E" после задач
            }
        }
        else if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            NextDialogueLine();
        }
        else if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            EndDialogueWithoutTasks(); // Прерываем диалог по "E"
        }
        else if (Input.GetKeyDown(KeyCode.J)) // Открытие задач по "J" в любой момент
        {
            ToggleTasks();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!isDialogueActive && interactionPrompt != null)
                interactionPrompt.SetActive(true); // Показываем "E" всегда, если диалог не активен
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false); // Скрываем "E" при выходе
            // Показываем стрелку только если диалог не завершён
            if (arrowIndicator != null && !isDialogueActive && !hasCompletedFirstDialogue)
                arrowIndicator.SetActive(true);
            if (isDialogueActive)
            {
                EndDialogueWithoutTasks(); // Прерываем диалог при выходе
            }
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (taskPanel != null) taskPanel.SetActive(false);
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueStep = 0; // Начинаем с первой реплики
        if (arrowIndicator != null) arrowIndicator.SetActive(false); // Стрелка пропадает
        if (interactionPrompt != null) interactionPrompt.SetActive(false); // "E" пропадает автоматически
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (subtitleText != null) subtitleText.text = dialogueLines[dialogueStep];
    }

    void NextDialogueLine()
    {
        dialogueStep++;
        if (dialogueStep < dialogueLines.Length)
        {
            if (subtitleText != null) subtitleText.text = dialogueLines[dialogueStep];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        hasCompletedFirstDialogue = true; // Диалог завершён
        hasShownTasks = true; // Задачи показаны
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (arrowIndicator != null) arrowIndicator.SetActive(false); // Отключаем маркер навсегда
        ShowTasks();
        if (interactionPrompt != null) interactionPrompt.SetActive(false); // Скрываем "E" после задач
    }

    void EndDialogueWithoutTasks()
    {
        isDialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        // Показываем стрелку только если диалог не завершён
        if (arrowIndicator != null && !hasCompletedFirstDialogue) arrowIndicator.SetActive(true);
    }

    void ShowTasks()
    {
        if (taskPanel != null && taskPanel.activeSelf)
        {
            taskPanel.SetActive(false); // Скрываем, если уже открыта
        }
        else if (taskPanel != null)
        {
            taskPanel.SetActive(true);
            TMP_Text taskText = taskPanel.GetComponentInChildren<TMP_Text>();
            if (taskText != null)
            {
                taskText.text = string.Join("\n", taskLines);
            }
        }
    }

    void ToggleTasks()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(!taskPanel.activeSelf); // Переключаем видимость
            if (taskPanel.activeSelf)
            {
                TMP_Text taskText = taskPanel.GetComponentInChildren<TMP_Text>();
                if (taskText != null)
                {
                    taskText.text = string.Join("\n", taskLines);
                }
            }
        }
    }
}