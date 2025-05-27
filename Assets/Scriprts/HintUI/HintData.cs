using UnityEngine;

public class HintData : MonoBehaviour
{
    [SerializeField] private string hintMessage = "Подсказка"; // Текст подсказки, редактируемый в Inspector

    public string GetHintText()
    {
        return hintMessage;
    }
}