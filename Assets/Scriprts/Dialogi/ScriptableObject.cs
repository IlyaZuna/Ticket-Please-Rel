using UnityEngine;

[System.Serializable]

public class DialogueLine
{
    public bool checkQwest = false;
    public int nextindexDialog = 0;
    public int nextQwestindexDialog = 0;
    [TextArea(3, 5)] public string text;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    public int QwestInt = 0;
    public string characterName;
    public DialogueLine[] lines;
}