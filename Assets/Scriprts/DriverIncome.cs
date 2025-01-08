using UnityEngine;

public class DriverIncome : MonoBehaviour
{
    private static DriverIncome _instance;  // Ссылка на экземпляр класса

    private int totalIncome;  // Текущий доход водителя

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
        Debug.Log($"Доход водителя увеличен на {amount}. Текущий доход: {totalIncome}");
    }

    // Метод для получения сдачи
    public int GetChange()
    {
        return totalIncome;  // Вернем текущий доход
    }

    // Метод для выдачи сдачи
    public void GiveChange(int change)
    {
        totalIncome -= change;
        Debug.Log($"Водитель отдал сдачу: {change}. Текущий доход: {totalIncome}");
        change = 0;
        MoneyGive = true;
    }
}
