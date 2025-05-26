using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ �� ������� �������
public class NPCInteraction : MonoBehaviour
{
    public DialogueLoader npcDialogue;
    public DialogueUI dialogueUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogueUI.ToggleDialogue(npcDialogue);
        }
    }
}
