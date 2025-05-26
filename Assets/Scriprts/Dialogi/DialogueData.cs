using UnityEngine;
[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(3, 5)] public string text;
    public int nextLineIndex; // -1 = ����� �������
    public bool isQuestTrigger; // �������������� �������
}

[System.Serializable]
public class DialogueData
{
    public DialogueLine[] lines;
}