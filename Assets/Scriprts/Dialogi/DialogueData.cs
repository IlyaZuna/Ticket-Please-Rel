using UnityEngine;
[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(3, 5)] public string text;
    public int nextLineIndex; // -1 = конец диалога
    public bool isQuestTrigger; // ƒополнительные услови€
}

[System.Serializable]
public class DialogueData
{
    public DialogueLine[] lines;
}