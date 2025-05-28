using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // Ссылка на экземпляр класса
    private int income = 0;                // Общий заработок (с начальным значением 0)
    private int totalChange = 0;           // Сумма сдачи
    [SerializeField] private int ticketPrice = 30;
    [SerializeField] private MoneySpawner coinSpawner;    // Спавнер для железных монет
    [SerializeField] private MoneySpawner paperMoneySpawner; // Спавнер для бумажных денег
    public static DriverIncome Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DriverIncome>();  // Находим компонент в сцене
            }
            return _instance;
        }
    }

    public bool MoneyGive = false;  // Если пассажир дал деньги

    public void AddIncome(int amount)
    {
        totalChange += amount;
        Debug.Log($"Сдача {amount}. Текущая сдача: {totalChange}");
    }

    public void Money(int _money, out bool _Sell)
    {
        if (Input.GetKeyDown(KeyCode.Q) && totalChange >= _money - ticketPrice)
        {
            _Sell = false;
            coinSpawner.ResetStack();       // Сброс спавнера монет
            paperMoneySpawner.ResetStack(); // Сброс спавнера бумажных денег
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