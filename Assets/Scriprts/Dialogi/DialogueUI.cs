using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("References")]
    public DialogueLoader dialogueLoader;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel; // Весь UI диалога
    public Button nextButton;
    public FirstPersonController player;

    [Header("Settings")]
    public bool unlockCursorOnDialogue = true;
    public bool lockCursorOnEnd = true;

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        // Скрываем UI при старте
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextLine);
    }

    // Запуск диалога (можно вызывать из других скриптов)
    public void StartDialogue(DialogueLoader dialogue)
    {
        if (isDialogueActive) return;
        player.LockStatePlayer();
        dialogueLoader = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        // Активируем UI
        dialoguePanel.SetActive(true);

        // Управление курсором
        if (unlockCursorOnDialogue)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Показываем первую реплику
        ShowLine(currentLineIndex);
    }

    // Завершение диалога
    public void EndDialogue()
    {
        if (!isDialogueActive) return;

        // Скрываем UI
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        // Возвращаем курсор
        if (lockCursorOnEnd)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Сбрасываем кнопку
        nextButton.interactable = true;
        player.LockStatePlayer();
    }

    void ShowLine(int index)
    {
        DialogueLine line = dialogueLoader.GetLine(index);
        if (line == null)
        {
            EndDialogue();
            return;
        }

        speakerText.text = line.speakerName;
        dialogueText.text = line.text;
        currentLineIndex = line.nextLineIndex;

        // Если диалог закончен
        if (currentLineIndex == -1)
        {
            nextButton.interactable = false;

            // Автозакрытие через 2 секунды (опционально)
            Invoke("EndDialogue", 2f);
        }
    }

    void ShowNextLine()
    {
        ShowLine(currentLineIndex);
    }

    // Для внешнего вызова (например, по нажатию E)
    public void ToggleDialogue(DialogueLoader dialogue)
    {
        if (isDialogueActive)
            EndDialogue();
        else
            StartDialogue(dialogue);
    }
}