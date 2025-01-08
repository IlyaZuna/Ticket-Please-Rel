using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerMove : MonoBehaviour
{
    [SerializeField]private float speed = 2f; // Скорость капсулы
    [SerializeField] private int stayIndex = 0; // Индекс точки, где капсула останавливается
    private KeyCode releaseKey = KeyCode.Return; // Клавиша для выхода из ожидания

    private Transform targetPoint; // Текущая цель
    private Point[] points; // Массив точек
    private Point2[] points2; // Массив точек
    private Point3[] points3; // Массив точек
    private int currentIndex = 0; // Текущий индекс точки
    private int RowExit = -1;

    private Animator animator;

    private bool isWaiting = false; // Флаг ожидания на точке
    private bool seat = false;
    private bool AnimSeat = false;
    private bool Spisok1 = false;
    private bool MoneyGive = false;
    [SerializeField] private bool _Inbus = true;
    [SerializeField] private bool _Outbus = false;

    private Transform childObject;
    private Transform parentObject;// Оплатил ли пассажир
    private GameObject[] billPrefabs; // Массив префабов для купюр
    private Transform spawnPoint;
    private int ticketPrice = 30; // Стоимость билета
    private int billGiven; // Купюра, которую даёт пассажир
    private BusStopTrigger busStopTrigger; // Для проверки находится ли пассажир на остановке
    private BusController busController;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;
    void Start()
    {
        // Находим все точки в сцене и сортируем их по индексу
        points = FindObjectsOfType<Point>();
        System.Array.Sort(points, (a, b) => a.Index.CompareTo(b.Index));
        points2 = FindObjectsOfType<Point2>();
        System.Array.Sort(points2, (a, b) => a.Index.CompareTo(b.Index));
        points3 = FindObjectsOfType<Point3>();
        System.Array.Sort(points3, (a, b) => a.Index.CompareTo(b.Index));
        busStopTrigger = FindObjectOfType<BusStopTrigger>();
        busController = FindObjectOfType<BusController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { if (_isAtBusStop && _areDoorsOpen)
        {
            if (_Inbus)
            {
                Gobus();
            }
            if (_Outbus)
            {
                Debug.Log("Выход");
                Outbus();
            }
        }
    }
    private void LateUpdate()
    {
        _isAtBusStop = busStopTrigger.isAtBusStop;
        _areDoorsOpen = busController.areDoorsOpen;
    }
    private void Gobus()
    {
        _Outbus = false;
        if (!isWaiting)
        {
            if (targetPoint == null && !seat && RowExit == -1) { SetNextTarget(); }
            if (seat && RowExit == -1) { SetSeatRowTarget(); }
            MoveToTarget();
        }
        else
        {
            PayForRide();
        }
        
        
        
    }
    private void Outbus()
    {
        _Inbus = false;
        seat = false;
        if (RowExit != -1 && !points2[RowExit].IsOccupied) {
            points2[RowExit].Occupy();
            targetPoint = points2[RowExit].transform; 
            
        } 
       
               
        MoveToTargetExit(); 
       
    }
    private void PayForRide()
    {
        // Случайным образом выбираем купюру (50, 100 или 200)
        if (MoneyGive == false)
        {
            billGiven = GetRandomBill();
            MoneyGive = true;
            DriverIncome.Instance.AddIncome(ticketPrice); // Добавляем доход водителю
            SpawnBill(billGiven);
            Debug.Log("Пассажир дал купюру: " + billGiven);
        }
        else
        {
            int change = billGiven - ticketPrice; // Рассчитываем сдачу
            Debug.Log("Оплата произведена! Сдача: " + change);

            // Получаем сдачу от водителя
            int driverChange = DriverIncome.Instance.GetChange();
            if (Input.GetKeyDown(KeyCode.Q) && driverChange > 0)
            {
                Debug.Log("Пассажир получил сдачу: " + driverChange);
                DriverIncome.Instance.GiveChange(driverChange); // Выдаем сдачу пассажиру
                Spisok1 = true;
                MoneyGive = false;
                return;// Помечаем, что пассажир оплатил
            }


        }
    }
    private int GetRandomBill()
    {
        int[] bills = { 50, 100, 200 };
        return bills[Random.Range(0, bills.Length)];
    }
    private void SpawnBill(int billAmount)
    {
        GameObject billPrefab = null;

        // Определяем, какой префаб выбрать на основе номинала купюры
        switch (billAmount)
        {
            case 50:
                billPrefab = billPrefabs[0]; // Префаб для купюры 50
                break;
            case 100:
                billPrefab = billPrefabs[1]; // Префаб для купюры 100
                break;
            case 200:
                billPrefab = billPrefabs[2]; // Префаб для купюры 200
                break;
        }

        if (billPrefab != null && spawnPoint != null)
        {
            // Спавним купюру и сохраняем ссылку на созданный объект
            GameObject spawnedBill = Instantiate(billPrefab, spawnPoint.position, spawnPoint.rotation);

            // Устанавливаем родителя для созданного объекта
            spawnedBill.transform.SetParent(parentObject.transform);

            Debug.Log("Купюра спавнена: " + billAmount);
        }
        else
        {
            Debug.LogWarning("Пропущен спавн купюры: billPrefab или spawnPoint не определены.");
        }
    }   
    private void MoveToTarget()
    {
        
        if(targetPoint == null) { return; }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.03f)
        {
            if (!seat && RowExit == -1)
            {
                Debug.Log(currentIndex);
                points[currentIndex].Release(); // Освобождаем текущую точку
                currentIndex++;
                SetNextTarget();
            }
            else if (currentIndex == stayIndex)
            {
                isWaiting = true; // Останавливаемся на точке ожидания
                Debug.Log($"Capsule reached stayIndex {stayIndex}. Waiting for trigger...");
            }
            else if (seat) 
            {
                
                points2[RowExit].Release();
                targetPoint = points3[currentIndex].transform;
                seat = false;

            }
            else
            {
                Debug.Log("сел");
                _Inbus = false;
                targetPoint = null;
                return;
            }


        }

    }
    private void SetNextTarget()
    {

        if (currentIndex < points.Length && !points[currentIndex].IsOccupied)
        {
            targetPoint = points[currentIndex].transform; // Устанавливаем цель
            points[currentIndex].Occupy(); // Занимаем точку         
        }
        else if(currentIndex >= points.Length)
        {
            seat = true;
            Debug.Log("seat");
            targetPoint = null;
        }
        else
        {
            targetPoint = null;
        }
    }
    private void SetSeatRowTarget()
    {
        targetPoint = null;
        currentIndex = Random.Range(0, 17);  // Присваиваем случайное значение, учитывая размер массива

        for (int i = 0; i < 36; i++)
        {
            if (!points3[currentIndex].IsOccupied)
            {                               
                

                switch (currentIndex)
                {
                    case >= 0 and <= 3:
                        RowExit = 0;
                        break;
                    case >= 4 and <= 7:
                        RowExit = 1;
                        break;
                    case >= 8 and <= 11:
                        RowExit = 2;
                        break;
                    case >= 12 and <= 15:
                        RowExit = 3;
                        break;
                    case >= 16 and <= 17:
                        RowExit = 4;
                        break;
                    default:
                        Debug.Log("Некорректный currentIndex в SetSeatRowMumberTarget!" + currentIndex);
                        break;
                }
                points2[RowExit].Occupy();
                points3[currentIndex].Occupy();
                targetPoint = points2[RowExit].transform;  // Устанавливаем цель
                Debug.Log("придумал"+RowExit+ "     currentIndex   " + currentIndex);
                return;
                
            }
            else
            {
                currentIndex++;
                if (currentIndex == points3.Length)
                {
                    currentIndex = 0;
                }
            }
        }
        if (targetPoint == null)
        {
            Debug.Log("не придумал");
        }


}
    private void MoveToTargetExit()
    {
        if (targetPoint == null) { return; }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.03f)
        {
            if(RowExit != -1)
            {
                points2[RowExit].Release();
                RowExit = -1;
            }
            if (targetPoint == points[0].transform)
            {
                targetPoint = null;
                _Outbus = false;
                return;
            }
            
            SetNextTargetExit();
        }
        
    }
    private void SetNextTargetExit()
    {
        if (!points[3].IsOccupied && targetPoint != points[3].transform)
        {
            points[3].Occupy();
            targetPoint = points[3].transform;
        }
        else if (!points[0].IsOccupied && targetPoint == points[3].transform && targetPoint != points[0].transform)
        {
            points[0].Occupy();
            points[3].Release();
            targetPoint = points[0].transform;
        }
        else
        {
            targetPoint = null;
        }
    }
}
