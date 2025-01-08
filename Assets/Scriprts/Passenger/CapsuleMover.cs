using UnityEngine;

public class CapsuleMover : MonoBehaviour
{
    public float speed = 2f; // Скорость капсулы
    public int stayIndex = 0; // Индекс точки, где капсула останавливается
    public KeyCode releaseKey = KeyCode.Return; // Клавиша для выхода из ожидания

    private Transform targetPoint; // Текущая цель
    private Point[] points; // Массив точек
    private Point2[] points2; // Массив точек
    private Point3[] points3; // Массив точек
    private int currentIndex = 0; // Текущий индекс точки
    private bool isWaiting = false; // Флаг ожидания на точке
    private bool seat = false;
    private int RowExit = -1;

    private Animator animator;

    
    public bool AnimSeat = false;
    public bool Spisok1 = false;
    public bool MoneyGive = false;

    public Transform childObject;
    public Transform parentObject;// Оплатил ли пассажир
    public GameObject[] billPrefabs; // Массив префабов для купюр
    public Transform spawnPoint;
    private int ticketPrice = 30; // Стоимость билета
    private int billGiven; // Купюра, которую даёт пассажир
    private BusStopTrigger busStopTrigger; // Для проверки находится ли пассажир на остановке
    private BusController busController;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;

    private void Start()
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

    private void Update()
    {
        if (AnimSeat)
        {
            bool isSitting = animator.GetBool("isSitting");
            animator.SetBool("isSitting", !isSitting);
            Debug.Log("БЛять");
            AnimSeat =false;

        }
        _isAtBusStop = busStopTrigger.isAtBusStop;
        _areDoorsOpen = busController.areDoorsOpen;       
        if (_isAtBusStop && _areDoorsOpen)
        {
            if (seat && RowExit != -1 && targetPoint == null)
            {
                Debug.Log("Капсула завершила своё движение.");
                return; // Прерываем Update
            }

            if (!seat)
            {
                if (isWaiting)
                {
                    PayForRide();
                    if (Spisok1)
                    {
                        isWaiting = false;
                        childObject.SetParent(parentObject);
                        points[currentIndex].Release();
                        currentIndex++;
                        SetNextTarget();
                        
                    }
                }

                if (targetPoint != null)
                {
                    MoveToTarget();
                }
                else if (currentIndex < points.Length)
                {
                    SetNextTarget();
                }
            }
            else
            {
                if (RowExit == -1)
                {
                    SetSeatRowTarget();
                    Debug.Log("застряло в 2 поинтах");
                }
                else
                {
                    MoveToTarget();
                }
            }
        }
    }


    /// <summary>
    /// Назначает следующую точку в качестве цели, если она есть и не занята.
    /// </summary>
    private void SetNextTarget()
    {

        if (currentIndex < points.Length && !points[currentIndex].IsOccupied)
        {
            targetPoint = points[currentIndex].transform; // Устанавливаем цель
            points[currentIndex].Occupy(); // Занимаем точку
            Debug.Log("Index" + currentIndex);
            if (currentIndex >= 5)
            {
                seat = true;
            }
        }
        else
        {
            targetPoint = null;
        }
    }

    /// <summary>
    /// Двигает капсулу к текущей цели и обрабатывает логику достижения точки.
    /// </summary>
    private void MoveToTarget()
    {
        if (targetPoint == null) return;
        

        // Движение к текущей точке
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Проверка достижения точки
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (seat && targetPoint == points3[RowExit].transform)
            {
                // Капсула достигла своей конечной цели
                Debug.Log($"Капсула достигла своей конечной цели: RowExit = {RowExit}");
                targetPoint = null; // Сбрасываем цель
                return; // Останавливаем движение
            }

            if (currentIndex == stayIndex)
            {
                isWaiting = true; // Останавливаемся на точке ожидания
                Debug.Log($"Capsule reached stayIndex {stayIndex}. Waiting for trigger...");
            }
            else
            {
                int nextIndex = currentIndex + 1;

                if (nextIndex < points.Length && !points[nextIndex].IsOccupied && !seat)
                {
                    points[currentIndex].Release(); // Освобождаем текущую точку
                    currentIndex = nextIndex; // Переходим к следующей
                    SetNextTarget(); // Устанавливаем новую цель
                }
                else if (seat)
                {
                    Debug.Log("DOSHOL");
                    SetSeatRowMumberTarget();
                }
            }

            
        }
        
        else
        {
            Vector3 directionToTarget = targetPoint.position - transform.position;

            // Вычисляем направление, противоположное цели
            Vector3 avoidanceDirection = -directionToTarget.normalized;

            // Вычисляем кватернион для нового направления
            Quaternion targetRotation = Quaternion.LookRotation(avoidanceDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private void SetSeatRowTarget()
    {
        points[currentIndex].Release();
        currentIndex = Random.Range(0, points2.Length);  // Присваиваем случайное значение, учитывая размер массива
        targetPoint = points2[currentIndex].transform;  // Устанавливаем цель
        RowExit = currentIndex;  // Устанавливаем RowExit
        Debug.Log(RowExit);

    }
    private void SetSeatRowMumberTarget()
    {
        switch (RowExit)
        {
            case 0:
                currentIndex = Random.Range(0, 3);
                break;
            case 1:
                currentIndex = Random.Range(4, 7);
                break;
            case 2:
                currentIndex = Random.Range(8, 11);
                break;
            case 3:
                currentIndex = Random.Range(12, 15);
                break;
            default:
                Debug.Log("Некорректный currentIndex в SetSeatRowMumberTarget!" + currentIndex);
                return;
        }

        int attempts = 0;
        const int maxAttempts = 8;

        while (points3[RowExit].IsOccupied && attempts < maxAttempts)
        {
            RowExit = Random.Range(0, points3.Length); // Генерация нового значения
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Не удалось найти свободную точку в SetSeatRowMumberTarget.");
            return;
        }

        points3[RowExit].Occupy(); // Занимаем точку
        targetPoint = points3[RowExit].transform; // Устанавливаем цель
        Debug.Log($"Капсула направляется к точке RowExit = {RowExit}");
        if (animator != null)
        {
            AnimSeat = true;
            Debug.Log("Анимка изменилась");
        }
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
}
