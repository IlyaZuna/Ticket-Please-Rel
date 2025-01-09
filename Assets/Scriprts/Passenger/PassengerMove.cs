using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerMove : MonoBehaviour
{
    [SerializeField]private float speed = 2f; // Скорость капсулы
    [SerializeField] private int stayIndex = 0; // Индекс точки, где капсула останавливается
    private KeyCode releaseKey = KeyCode.Return; // Клавиша для выхода из ожидания

    private Transform targetPoint = null; // Текущая цель
    private Point[] points; // Массив точек
    private Point2[] points2; // Массив точек
    private Point3[] points3; // Массив точек
    private int currentIndex = 0; // Текущий индекс точки
    private int RowExit = -1;
    private int change = 0;
    public float rotationSpeed = 5f;
    public int driverChange;

    private bool isWaiting = false; // Флаг ожидания на точке
    private bool seat = false;   
    private bool MoneyGive = false;
    [SerializeField] private bool _Inbus = true;
    [SerializeField] private bool _Outbus = false;
    [SerializeField]public AnimBase animator;
    [SerializeField] private Transform childObject;
    [SerializeField] private Transform parentObject;// Оплатил ли пассажир
    [SerializeField]private GameObject[] billPrefabs; // Массив префабов для купюр
    private GameObject billPrefab;
    private GameObject spawnedBill;
    [SerializeField] private Transform spawnPoint;
    private int ticketPrice = 30; // Стоимость билета
    private int billGiven; // Купюра, которую даёт пассажир
    private BusStopTrigger busStopTrigger; // Для проверки находится ли пассажир на остановке
    private BusController busController;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;
    private bool seattrue = false;
    private int _indexBusStop = -1;
    [SerializeField] private int _indexOUT;
    [SerializeField] private int _indexSpawn;
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
        

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("!!!!!!!!!!!!!_indexSpawn" + _indexSpawn);
        Debug.Log("!!!!!!!!!!!!!_indexSpawn" + _indexBusStop);
        if (_isAtBusStop && _areDoorsOpen)
        {
            if (_Inbus)
            {
                Gobus();
            }
            if (_Outbus)
            {
                
                Outbus();
            }
        }       
        if(targetPoint == null && !seattrue)
        {
            animator.Idle();
        }
        if (isWaiting)
        {
            animator.Idle();
        }
        else if (!seattrue && targetPoint !=null)
        {
            Vector3 direction = new Vector3(targetPoint.position.x - transform.position.x, 0f, targetPoint.position.z - transform.position.z);


            if (direction.sqrMagnitude > 0.01f) // проверка на нулевое расстояние
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            animator.Walk();
        }     
        if(!_Outbus && !_Inbus)
        {
            seattrue = true;
            animator.Sit();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, -180f + parentObject.eulerAngles.y, 0f), Time.deltaTime * rotationSpeed);
        }

        if(_indexBusStop != -1 && _indexBusStop == _indexOUT)
        {
            _Outbus = true;
        }
    }
    private void LateUpdate()
    {
        _isAtBusStop = busController.s;
        _areDoorsOpen = busController.areDoorsOpen;
        _indexBusStop = busController.currentStopIndex;
        Debug.Log(_indexBusStop + "_indexBusStop");
        //_indexBusStop = busStopTrigger.indexStop;
    }
    private void Gobus()
    {
        _Outbus = false;
        if (!isWaiting)
        {   if(targetPoint == points[2].transform) { childObject.SetParent(parentObject); }
            if (targetPoint == null && !seat && RowExit == -1) { SetNextTarget(); }
            if (seat && RowExit == -1) { SetSeatRowTarget(); }
            MoveToTarget();
        }
        else if(isWaiting)
        {          
            PayForRide();
        }
       
        
        
        
    }
    private void Outbus()
    {
        _Inbus = false;
        seattrue = false;
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
            SpawnBill(billGiven);
            Debug.Log("Пассажир дал купюру: " + billGiven);
            change = billGiven - ticketPrice; // Рассчитываем сдачу
        }        
        Debug.Log("Оплата произведена! Сдача: " + change);   
        driverChange = DriverIncome.Instance.GetChange();//ТЕКУЩАЯ СДАЧА
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (driverChange >= change) 
            {
                //DriverIncome.Instance.AddIncome(ticketPrice);
                Debug.Log("Пассажир получил сдачу: " + driverChange);
                DriverIncome.Instance.GivepASAJChange(driverChange); // Выдаем сдачу пассажиру                
                isWaiting = false;
                points[currentIndex].Release();
                Destroy(spawnedBill);
                
                return;// Помечаем, что пассажир оплатил
            }
            else
            {
                Debug.Log("Мало");
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
            default:
                break;
        }

        if (billPrefab != null && spawnPoint != null)
        {
            // Спавним купюру и сохраняем ссылку на созданный объект
            spawnedBill = Instantiate(billPrefab, spawnPoint.position, spawnPoint.rotation);

            // Устанавливаем родителя для созданного объекта
            spawnedBill.transform.SetParent(parentObject.transform);
            spawnedBill.transform.localScale = new Vector3(20, 10, 20);  // Затем изменить масштаб
            
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
            if (currentIndex == stayIndex && !MoneyGive)
            {
                isWaiting = true; // Останавливаемся на точке ожидания
                Debug.Log($"Capsule reached stayIndex {stayIndex}. Waiting for trigger...");
            }
            if (!seat && RowExit == -1 && !isWaiting)
            {

               
                    points[currentIndex].Release(); // Освобождаем текущую точку
                    currentIndex++;
                    SetNextTarget();
               


            }          
            else if (seat && !isWaiting) 
            {
                
                points2[RowExit].Release();
                targetPoint = points3[currentIndex].transform;
                seat = false;

            }
            else if(targetPoint == points3[currentIndex].transform)
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

        if (currentIndex < points.Length && !points[currentIndex].IsOccupied )
        {
            targetPoint = points[currentIndex].transform; // Устанавливаем цель
            points[currentIndex].Occupy(); // Занимаем точку         
        }
        else if(currentIndex >= points.Length)
        {
            seat = true;
            
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
                childObject.SetParent(null);
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
