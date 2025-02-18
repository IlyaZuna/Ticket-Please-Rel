using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // ������ �� ��������� ������

    private int totalIncome;
    [SerializeField] private MoneySpawner moneySpawner; // ������ �� ������-�������
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
        Debug.Log($"������ �������� �������� �� {amount}. ������� �����: {totalIncome}");
    }

    // ����� ��� ��������� �����
    public int GetChange()
    {
        return totalIncome;  
    }   

    // ����� ��� ������ �����
    public void GiveChange(int billValue)
    {
        totalIncome += billValue; // ����������� �����
        Debug.Log($"����� ��������� �� {billValue}. ������� �����: {totalIncome}");

    }
    // ����� ��� ������ �����
    public void GivepASAJChange(int change)
    {
        totalIncome = 0;
        Debug.Log($"�������� ����� �����: {change}. ������� �����: {totalIncome}");
        moneySpawner.ResetStack();
        MoneyGive = true;
    }
}
