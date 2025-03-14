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
    }

    public void DisplayNextLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        nameText.text = currentDialogue.lines[currentLineIndex].characterName;
        dialogueText.text = currentDialogue.lines[currentLineIndex].text;
        currentLineIndex++;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        player.LockStatePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
