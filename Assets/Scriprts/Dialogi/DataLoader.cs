using UnityEngine;

public class DataLoader : MonoBehaviour ,IInteractable
{
    public DialogManager dialogueManager;
    public DialogueData myDialogue;
    public DialogueData nextmyDialogue;
    public GameObject interactionPR;

    public void Interact()
    {
        dialogueManager.StartDialogue(myDialogue);
        interactionPR.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {        
            interactionPR.SetActive(true); // ѕоказываем "E" всегда, если диалог не активен
    }
    void OnTriggerExit(Collider other)
    {
        interactionPR.SetActive(false);
    }
    public void CwichDialog()
    {
        myDialogue = nextmyDialogue;
    }
}