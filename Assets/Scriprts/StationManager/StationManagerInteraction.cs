using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StationManagerInteraction : MonoBehaviour
{
    public GameObject interactionPrompt; // ��������� "E - �����������������"
    public GameObject arrowIndicator; // ���������� ������� ��� �����������
    public GameObject dialoguePanel; // ������ ��� ���������
    public TMP_Text subtitleText; // ����� ���������
    public GameObject taskPanel; // ������ � ��������
    private bool isPlayerInRange = false; // ���������, ��������� �� ����� �����
    private bool isDialogueActive = false; // ������� �� ������
    private bool hasCompletedFirstDialogue = false; // �������� �� ������ ������ ���������
    private int dialogueStep = 0; // ������� ��� �������
    private bool hasShownTasks = false; // �������� �� ������

    // ������� ����������
    private string[] dialogueLines = new string[]
    {
        "��������, � ��������� ����� ��������� ����� ����� �� ����������...",
        "������������ ���������� ����� ����� �������� ������� �������� ���������������� ���������� (����).",
        "������ �� ������ ������� �� ������� �������� ���������� (���) � ������� ��� ������������!",
        "������ ����������� ������������ � ��� � ����� ������. ������ �������� ������� ������ � �������!",
        "��� �������� ���� �������� �� ����� ������� � ������ �������� �� ����!"
    };

    // ������
    private string[] taskLines = new string[]
    {
        "������ �� �����:",
        "- ��������� 20 ����������",
        "- ������� ��� > 50%"
    };

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (arrowIndicator != null) arrowIndicator.SetActive(true); // ������� ������� � ������
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
                ShowTasks(); // ���������� ������, ���� ��� ��� ���� ��������
                if (interactionPrompt != null) interactionPrompt.SetActive(false); // �������� "E" ����� �����
            }
        }
        else if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            NextDialogueLine();
        }
        else if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            EndDialogueWithoutTasks(); // ��������� ������ �� "E"
        }
        else if (Input.GetKeyDown(KeyCode.J)) // �������� ����� �� "J" � ����� ������
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
                interactionPrompt.SetActive(true); // ���������� "E" ������, ���� ������ �� �������
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false); // �������� "E" ��� ������
            // ���������� ������� ������ ���� ������ �� ��������
            if (arrowIndicator != null && !isDialogueActive && !hasCompletedFirstDialogue)
                arrowIndicator.SetActive(true);
            if (isDialogueActive)
            {
                EndDialogueWithoutTasks(); // ��������� ������ ��� ������
            }
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (taskPanel != null) taskPanel.SetActive(false);
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueStep = 0; // �������� � ������ �������
        if (arrowIndicator != null) arrowIndicator.SetActive(false); // ������� ���������
        if (interactionPrompt != null) interactionPrompt.SetActive(false); // "E" ��������� �������������
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
        hasCompletedFirstDialogue = true; // ������ ��������
        hasShownTasks = true; // ������ ��������
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (arrowIndicator != null) arrowIndicator.SetActive(false); // ��������� ������ ��������
        ShowTasks();
        if (interactionPrompt != null) interactionPrompt.SetActive(false); // �������� "E" ����� �����
    }

    void EndDialogueWithoutTasks()
    {
        isDialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        // ���������� ������� ������ ���� ������ �� ��������
        if (arrowIndicator != null && !hasCompletedFirstDialogue) arrowIndicator.SetActive(true);
    }

    void ShowTasks()
    {
        if (taskPanel != null && taskPanel.activeSelf)
        {
            taskPanel.SetActive(false); // ��������, ���� ��� �������
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
            taskPanel.SetActive(!taskPanel.activeSelf); // ����������� ���������
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