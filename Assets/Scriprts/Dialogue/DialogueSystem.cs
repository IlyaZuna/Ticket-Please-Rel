using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel; // UI-панель диалога
    [SerializeField] private TextMeshProUGUI dialogueText; // Текст диалога
    [SerializeField] private TextMeshProUGUI characterNameText; // Текстовое поле для имени персонажа
    [SerializeField] private Button nextButton; // Кнопка "Далее"

    private string[] dialogueLines; // Массив реплик
    private int currentLineIndex = 0; // Текущая реплика
    private string characterName; // Имя персонажа, переданное из CharacterInteractable
    private System.Action onDialogueEnd; // Действие после завершения диалога
    private HintSystem hintSystem; // Ссылка на HintSystem
    private GameObject lastInteractedTarget; // Последний объект, с которым взаимодействовали
    private Camera playerCamera; // Камера игрока

    void Start()
    {
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextLine);

        // Находим HintSystem в сцене
        hintSystem = FindObjectOfType<HintSystem>();
        if (hintSystem == null)
        {
            Debug.LogError("HintSystem not found in scene!");
        }

        // Находим FirstPersonController и берём его камеру
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerCamera = playerController.GetComponentInChildren<Camera>();
            if (playerCamera == null)
            {
                Debug.LogError("Player Camera not found in FirstPersonController!");
            }
        }
        else
        {
            Debug.LogError("FirstPersonController not found in scene!");
        }
    }

    // Запуск диалога с именем персонажа
    public void StartDialogue(string characterName, string[] lines, GameObject target, System.Action onEnd = null)
    {
        this.characterName = characterName;
        dialogueLines = lines;
        currentLineIndex = 0;
        onDialogueEnd = onEnd;
        lastInteractedTarget = target; // Сохраняем объект, с которым начали диалог

        // Настраиваем панель диалога
        characterNameText.text = characterName; // Устанавливаем имя в отдельное поле
        dialogueText.text = dialogueLines[currentLineIndex]; // Только текст реплики
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f; // Ставим игру на паузу

        // Делаем курсор видимым и разблокируем его
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Скрываем подсказку при начале диалога
        if (hintSystem != null)
        {
            hintSystem.HideHint();
        }

        // Явно устанавливаем фокус на кнопку "Дальше"
        EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
    }

    // Показ следующей реплики
    private void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = dialogueLines[currentLineIndex];
            characterNameText.text = characterName; // Обновляем имя
        }
        else
        {
            EndDialogue();
        }
    }

    // Завершение диалога
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; // Возвращаем игру
        // Скрываем курсор и блокируем его
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Показываем подсказку снова, если игрок всё ещё смотрит на объект
        if (hintSystem != null && lastInteractedTarget != null && playerCamera != null)
        {
            // Проверяем, смотрит ли игрок на тот же объект
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, 5f)) // 5f — дистанция взаимодействия
            {
                if (hit.collider.gameObject == lastInteractedTarget)
                {
                    hintSystem.ShowHint(lastInteractedTarget);
                }
            }
        }

        onDialogueEnd?.Invoke();
    }
}