using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // ������ �� ��������� ������

    private int totalIncome;  // ������� ����� ��������

    // ����������� �������� ��� ��������� ������������� ���������� ������
    public static DriverIncome Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DriverIncome>();  // ������� ��������� � �����, ���� �� ��� �� ������
            }
            return _instance;
        }
    }

    public bool MoneyGive = false;  // ���� �������� ��� ������

    // ����� ��� ���������� ������
    public void AddIncome(int amount)
    {
        totalIncome += amount;
        Debug.Log($"����� �������� �������� �� {amount}. ������� �����: {totalIncome}");
    }

    // ����� ��� ��������� �����
    public int GetChange()
    {
        return totalIncome;  // ������ ������� �����
    }

    // ����� ��� ������ �����
    public void GiveChange(int change)
    {
        totalIncome -= change;
        Debug.Log($"�������� ����� �����: {change}. ������� �����: {totalIncome}");
        change = 0;
        MoneyGive = true;
    }
}
