using UnityEngine;

public class DataLoader : MonoBehaviour, IInteractable
{
    [Header("Config")]
    [SerializeField] private string playerTag = "Player";

    [Header("References")]
    [SerializeField] private DialogueData myDialogue;
    [SerializeField] private DialogueData nextmyDialogue;
    [SerializeField] private GameObject interactionPR;

    [SerializeField] private DialogManager dialogueManager;
    [SerializeField] private bool canInteract;

    private void Start()
    {
        // Автопоиск менеджера (для билда)
        dialogueManager = FindObjectOfType<DialogManager>();
        if (dialogueManager == null)
            Debug.LogError("DialogManager not found!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canInteract = true;
            interactionPR.SetActive(true);
            dialogueManager?.SetCurrentDataLoader(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canInteract = false;
            interactionPR.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!canInteract || dialogueManager == null) return;

        dialogueManager.StartDialogue(myDialogue);
        interactionPR.SetActive(false);
    }

    public void SwitchDialog()
    {
        if (nextmyDialogue != null)
            myDialogue = nextmyDialogue;
    }
}