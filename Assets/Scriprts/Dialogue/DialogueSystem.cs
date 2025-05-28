using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel; // UI-������ �������
    [SerializeField] private TextMeshProUGUI dialogueText; // ����� �������
    [SerializeField] private TextMeshProUGUI characterNameText; // ��������� ���� ��� ����� ���������
    [SerializeField] private Button nextButton; // ������ "�����"

    private string[] dialogueLines; // ������ ������
    private int currentLineIndex = 0; // ������� �������
    private string characterName; // ��� ���������, ���������� �� CharacterInteractable
    private System.Action onDialogueEnd; // �������� ����� ���������� �������
    private HintSystem hintSystem; // ������ �� HintSystem
    private GameObject lastInteractedTarget; // ��������� ������, � ������� �����������������
    private Camera playerCamera; // ������ ������

    void Start()
    {
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextLine);

        // ������� HintSystem � �����
        hintSystem = FindObjectOfType<HintSystem>();
        if (hintSystem == null)
        {
            Debug.LogError("HintSystem not found in scene!");
        }

        // ������� FirstPersonController � ���� ��� ������
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

    // ������ ������� � ������ ���������
    public void StartDialogue(string characterName, string[] lines, GameObject target, System.Action onEnd = null)
    {
        this.characterName = characterName;
        dialogueLines = lines;
        currentLineIndex = 0;
        onDialogueEnd = onEnd;
        lastInteractedTarget = target; // ��������� ������, � ������� ������ ������

        // ����������� ������ �������
        characterNameText.text = characterName; // ������������� ��� � ��������� ����
        dialogueText.text = dialogueLines[currentLineIndex]; // ������ ����� �������
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f; // ������ ���� �� �����

        // ������ ������ ������� � ������������ ���
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // �������� ��������� ��� ������ �������
        if (hintSystem != null)
        {
            hintSystem.HideHint();
        }

        // ���� ������������� ����� �� ������ "������"
        EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
    }

    // ����� ��������� �������
    private void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = dialogueLines[currentLineIndex];
            characterNameText.text = characterName; // ��������� ���
        }
        else
        {
            EndDialogue();
        }
    }

    // ���������� �������
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; // ���������� ����
        // �������� ������ � ��������� ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ���������� ��������� �����, ���� ����� �� ��� ������� �� ������
        if (hintSystem != null && lastInteractedTarget != null && playerCamera != null)
        {
            // ���������, ������� �� ����� �� ��� �� ������
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, 5f)) // 5f � ��������� ��������������
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