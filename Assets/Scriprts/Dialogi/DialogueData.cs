using UnityEngine;

[System.Serializable]

public class DialogueLine
{
    [SerializeField] public bool checkQwest = false;
    [SerializeField] public int nextindexDialog = 0;
    [SerializeField] public int nextQwestindexDialog = 0;
    [TextArea(3, 5)] public string text;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    [SerializeField] public int QwestInt = 0;
    [SerializeField] public string characterName;
    [SerializeField] public DialogueLine[] lines;
}