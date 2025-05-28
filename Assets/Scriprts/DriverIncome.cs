using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // Ссылка на экземпляр класса
    private int income;
    private int totalChange;
    [SerializeField] private int ticketPrice = 30;
    [SerializeField] private MoneySpawner moneySpawner; // Ссылка на объект-спавнер    
    [SerializeField] private MoneySpawner MmoneySpawner; // Ссылка на объект-спавнер    
    public static DriverIncome Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DriverIncome>();  // Находим компонент в сцене, если он еще не найден
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

            moneySpawner.ResetStack();
            MmoneySpawner.ResetStack();
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
        return (income);

    }

}

