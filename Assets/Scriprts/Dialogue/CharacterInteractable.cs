using UnityEngine;

public class CharacterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] dialogueLines; // Реплики диалога
    [SerializeField] private DialogueSystem dialogueSystem; // Ссылка на систему диалогов
    [SerializeField] private string characterName; // Имя персонажа
    private FirstPersonController playerController; // Ссылка на контроллер игрока
    private System.Action onDialogueEnd; // Действие после диалога

    void Start()
    {
        if (dialogueSystem == null)
        {
            dialogueSystem = FindObjectOfType<DialogueSystem>();
            if (dialogueSystem == null)
            {
                Debug.LogError("DialogueSystem not found in scene!");
            }
        }

        playerController = FindObjectOfType<FirstPersonController>();
        if (playerController == null)
        {
            Debug.LogError("FirstPersonController not found in scene!");
        }

        // Пример: Логика на основе тега
        if (gameObject.CompareTag("Chief"))
        {
            onDialogueEnd = () =>
            {
                Debug.Log("Диалог с Шефом завершён. Выдаём задание...");
            };
        }
        else if (gameObject.CompareTag("Passenger"))
        {
            onDialogueEnd = () =>
            {
                Debug.Log("Диалог с Пассажиром завершён. Добавляем его в маршрут...");
            };
        }
    }

    public void Interact()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(characterName, dialogueLines, gameObject, () =>
            {
                // Разблокируем игрока после завершения диалога
                if (playerController != null)
                {
                    playerController.LockStatePlayer();
                }
                onDialogueEnd?.Invoke(); // Вызываем действие после диалога
            });
        }
    }

    // Метод для установки действия после диалога (например, для задания)
    public void SetOnDialogueEnd(System.Action action)
    {
        onDialogueEnd = action;
    }
}