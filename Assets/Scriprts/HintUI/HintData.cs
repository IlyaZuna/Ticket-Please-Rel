using UnityEngine;

public class HintData : MonoBehaviour
{
    [SerializeField] private string hintMessage = "���������"; // ����� ���������, ������������� � Inspector

    public string GetHintText()
    {
        return hintMessage;
    }
}