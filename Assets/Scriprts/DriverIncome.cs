using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // ������ �� ��������� ������
    private int income = 0;                // ����� ��������� (� ��������� ��������� 0)
    private int totalChange = 0;           // ����� �����
    [SerializeField] private int ticketPrice = 30;
    [SerializeField] private MoneySpawner coinSpawner;    // ������� ��� �������� �����
    [SerializeField] private MoneySpawner paperMoneySpawner; // ������� ��� �������� �����
    public static DriverIncome Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DriverIncome>();  // ������� ��������� � �����
            }
            return _instance;
        }
    }

    public bool MoneyGive = false;  // ���� �������� ��� ������

    public void AddIncome(int amount)
    {
        totalChange += amount;
        Debug.Log($"����� {amount}. ������� �����: {totalChange}");
    }

    public void Money(int _money, out bool _Sell)
    {
        if (Input.GetKeyDown(KeyCode.Q) && totalChange >= _money - ticketPrice)
        {
            _Sell = false;
            coinSpawner.ResetStack();       // ����� �������� �����
            paperMoneySpawner.ResetStack(); // ����� �������� �������� �����
            income = income - totalChange + _money;
            totalChange = 0;
            return;
        }
        _Sell = true;
    }

    public int GetIncam()
    {
        return income;
    }

    public int Incame()
    {
        return income;
    }
}