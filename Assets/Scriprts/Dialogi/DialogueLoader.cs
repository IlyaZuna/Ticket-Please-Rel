using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    [Header("JSON Data")]
    public TextAsset jsonFile; // Перетащите сюда JSON-файл

    [Header("Debug View")]
    [SerializeField] private DialogueData dialogueData; // Данные после загрузки

    // Загружает JSON автоматически при изменении в инспекторе
    void OnValidate()
    {
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        }
    }

    // Для ручной перезагрузки в runtime
    public void ReloadData()
    {
        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        }
    }

    // Пример: получение строки по индексу
    public DialogueLine GetLine(int index)
    {
        if (dialogueData == null || index < 0 || index >= dialogueData.lines.Length)
            return null;

        return dialogueData.lines[index];
    }
}