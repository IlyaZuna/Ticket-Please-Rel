using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialoguePanel;
    public Button nextButton;
    public ThirdPersonController player;
    public ManagerStats stats;
    private DialogueData currentDialogue;
    private int currentLineIndex;

    void Start()
    {
        dialoguePanel.SetActive(false); // Диалог скрыт при старте
        nextButton.onClick.AddListener(DisplayNextLine);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.LockStatePlayer();
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayNextLine();
        Debug.Log(stats.CheckQwest(currentDialogue.QwestInt));
    }

    public void DisplayNextLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        nameText.text = currentDialogue.characterName;
        dialogueText.text = currentDialogue.lines[currentLineIndex].text;


        if (currentDialogue.lines[currentLineIndex].checkQwest && stats.CheckQwest(currentDialogue.QwestInt))
        {
            currentLineIndex = currentDialogue.lines[currentLineIndex].nextQwestindexDialog;
        }
        else if (currentDialogue.lines[currentLineIndex].nextindexDialog != 0)
        {
            currentLineIndex = currentDialogue.lines[currentLineIndex].nextindexDialog;
        }
        else
        {
            currentLineIndex++;
        }

    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        player.LockStatePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
