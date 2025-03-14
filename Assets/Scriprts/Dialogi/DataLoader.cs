using UnityEngine;

public class DataLoader : MonoBehaviour ,IInteractable
{
    public DialogManager dialogueManager;
    public DialogueData myDialogue;
    public GameObject interactionPR;

    public void Interact()
    {
        dialogueManager.StartDialogue(myDialogue);
        interactionPR.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {        
            interactionPR.SetActive(true); // ���������� "E" ������, ���� ������ �� �������
    }
    void OnTriggerExit(Collider other)
    {
        interactionPR.SetActive(false);
    }
}