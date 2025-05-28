using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStats : MonoBehaviour
{
    public int _pasengerSkore;      // Пассажиры за текущую смену
    public int _pasengerallSkore;   // Все пассажиры за всё время
    public int day;                 // Номер дня
    private DriverIncome driverIncome; // Ссылка на DriverIncome
    public int allIncame;           // Общий доход

    void Start()
    {
        driverIncome = FindObjectOfType<DriverIncome>(); // Инициализация DriverIncome
        if (driverIncome == null)
        {
            Debug.LogError("DriverIncome not found in scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPasengerSkore()
    {
        _pasengerSkore++;
        return;
    }

    public void addPasengerallSkore()
    {
        _pasengerallSkore++;
        return;
    }

    public void addDay()
    {
        day++;
        addPasengerallSkore();
        allIncame = driverIncome != null ? driverIncome.Incame() : 0; // Безопасная проверка
        return;
    }

    public int getPasengerSkore()
    {
        return _pasengerSkore;
    }

    // Заглушка для уровня УКП (например, от 1 до 5)
    public int GetUKPLevel()
    {
        return 100; // Заглушка, можно заменить на реальную логику
    }
}