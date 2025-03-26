using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Button nextButton;

    [Header("Dependencies")]
    [SerializeField] private FirstPersonController player;
    [SerializeField] private ManagerStats stats;

    private DialogueData currentDialogue;
    private DataLoader currentLoader;
    private int currentLineIndex;

    public void SetCurrentDataLoader(DataLoader loader) => currentLoader = loader;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(DisplayNextLine);

        // Автопоиск если не назначено
        if (player == null) player = FindObjectOfType<FirstPersonController>();
        if (stats == null) stats = FindObjectOfType<ManagerStats>();
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player?.LockStatePlayer();
        dialoguePanel.SetActive(true);

        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentDialogue.lines[currentLineIndex];
        nameText.text = currentDialogue.characterName;
        dialogueText.text = line.text;

        if (line.checkQwest && stats.CheckQwest(currentDialogue.QwestInt))
        {
            currentLineIndex = line.nextQwestindexDialog;
            currentLoader?.SwitchDialog();
        }
        else
        {
            currentLineIndex = line.nextindexDialog != 0 ? line.nextindexDialog : currentLineIndex + 1;
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        player?.LockStatePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}