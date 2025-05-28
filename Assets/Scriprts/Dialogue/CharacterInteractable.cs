using UnityEngine;

public class CharacterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] dialogueLines; // ������� �������
    [SerializeField] private DialogueSystem dialogueSystem; // ������ �� ������� ��������
    [SerializeField] private string characterName; // ��� ���������
    private FirstPersonController playerController; // ������ �� ���������� ������
    private System.Action onDialogueEnd; // �������� ����� �������

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

        // ������: ������ �� ������ ����
        if (gameObject.CompareTag("Chief"))
        {
            onDialogueEnd = () =>
            {
                Debug.Log("������ � ����� ��������. ����� �������...");
            };
        }
        else if (gameObject.CompareTag("Passenger"))
        {
            onDialogueEnd = () =>
            {
                Debug.Log("������ � ���������� ��������. ��������� ��� � �������...");
            };
        }
    }

    public void Interact()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(characterName, dialogueLines, gameObject, () =>
            {
                // ������������ ������ ����� ���������� �������
                if (playerController != null)
                {
                    playerController.LockStatePlayer();
                }
                onDialogueEnd?.Invoke(); // �������� �������� ����� �������
            });
        }
    }

    // ����� ��� ��������� �������� ����� ������� (��������, ��� �������)
    public void SetOnDialogueEnd(System.Action action)
    {
        onDialogueEnd = action;
    }
}