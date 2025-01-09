using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // Ссылка на экземпляр класса

    private int totalIncome;
    [SerializeField] private MoneySpawner moneySpawner; // Ссылка на объект-спавнер
    // Статическое свойство для получения единственного экземпляра класса
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

    // Метод для увеличения дохода
    public void AddIncome(int amount)
    {
        totalIncome += amount;
        Debug.Log($"дениги водителя увеличен на {amount}. Текущий доход: {totalIncome}");
    }

    // Метод для получения сдачи
    public int GetChange()
    {
        return totalIncome;  
    }   

    // Метод для выдачи сдачи
    public void GiveChange(int billValue)
    {
        totalIncome += billValue; // Увеличиваем сдачу
        Debug.Log($"Сдача увеличена на {billValue}. Текущая сдача: {totalIncome}");

    }
    // Метод для выдачи сдачи
    public void GivepASAJChange(int change)
    {
        totalIncome = 0;
        Debug.Log($"Водитель отдал сдачу: {change}. Текущий доход: {totalIncome}");
        moneySpawner.ResetStack();
        MoneyGive = true;
    }
}
