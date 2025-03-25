// ScriptableObject для хранения диалога
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 5)] public string text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}