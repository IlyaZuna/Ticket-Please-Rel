using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("References")]
    public DialogueLoader dialogueLoader;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel; // ���� UI �������
    public Button nextButton;
    public FirstPersonController player;

    [Header("Settings")]
    public bool unlockCursorOnDialogue = true;
    public bool lockCursorOnEnd = true;

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        // �������� UI ��� ������
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextLine);
    }

    // ������ ������� (����� �������� �� ������ ��������)
    public void StartDialogue(DialogueLoader dialogue)
    {
        if (isDialogueActive) return;
        player.LockStatePlayer();
        dialogueLoader = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        // ���������� UI
        dialoguePanel.SetActive(true);

        // ���������� ��������
        if (unlockCursorOnDialogue)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // ���������� ������ �������
        ShowLine(currentLineIndex);
    }

    // ���������� �������
    public void EndDialogue()
    {
        if (!isDialogueActive) return;

        // �������� UI
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        // ���������� ������
        if (lockCursorOnEnd)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // ���������� ������
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

        // ���� ������ ��������
        if (currentLineIndex == -1)
        {
            nextButton.interactable = false;

            // ������������ ����� 2 ������� (�����������)
            Invoke("EndDialogue", 2f);
        }
    }

    void ShowNextLine()
    {
        ShowLine(currentLineIndex);
    }

    // ��� �������� ������ (��������, �� ������� E)
    public void ToggleDialogue(DialogueLoader dialogue)
    {
        if (isDialogueActive)
            EndDialogue();
        else
            StartDialogue(dialogue);
    }
}