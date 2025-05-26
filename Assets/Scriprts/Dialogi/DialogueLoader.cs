using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    [Header("JSON Data")]
    public TextAsset jsonFile; // ���������� ���� JSON-����

    [Header("Debug View")]
    [SerializeField] private DialogueData dialogueData; // ������ ����� ��������

    // ��������� JSON ������������� ��� ��������� � ����������
    void OnValidate()
    {
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        }
    }

    // ��� ������ ������������ � runtime
    public void ReloadData()
    {
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        }
    }

    // ������: ��������� ������ �� �������
    public DialogueLine GetLine(int index)
    {
        if (dialogueData == null || index < 0 || index >= dialogueData.lines.Length)
            return null;

        return dialogueData.lines[index];
    }
}