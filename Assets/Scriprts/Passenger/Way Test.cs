using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayTest : MonoBehaviour
{
    private Transform target;
    private FindWay findWay;
    private int index = 0;
    [SerializeField] private float speed = 5f;
    private int RowExit = -2;
    public float rotationSpeed = 5f;
    [SerializeField] private int stayIndex = 4;
    [SerializeField] private bool isWaiting = false;
    [SerializeField] private bool MoneyGive = false;
    [SerializeField] private bool seat = false;
    [SerializeField] private bool goseat = false;
    [SerializeField] public AnimBase animator;
    private NavMeshAgent agent;


    [SerializeField] private GameObject[] billPrefabs; // Массив префабов для купюр
    [SerializeField] private Transform spawnPointMoneyPasajira;
    private GameObject billPrefab;
    private GameObject spawnedBill;
    private int billGiven; // Купюра, которую даёт пассажир



    public int driverChange;



    [SerializeField] private Transform childObject;
    [SerializeField] private Transform parentObject;// Оплатил ли пассажир


    [SerializeField] private bool _Inbus = true;
    [SerializeField] private bool _Outbus = false;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;
    private BusController busController;

    private int _indexBusStop = -1;
    [SerializeField] private int _indexSpawn = 0;
    [SerializeField] private int _indexOUT;

    void Start()
    {
        busController = FindObjectOfType<BusController>();
        agent = GetComponent<NavMeshAgent>();
        findWay = FindObjectOfType<FindWay>();


        ButtonDoor.OnButtonPressed += ToggleDoor;
    }
    void Update()
    {
        AnimationSost();
        if (!_Inbus && !_Outbus)
        {
            Walk();
            return;
        }
        if (_indexBusStop > _indexOUT && _indexOUT != 0)
        {
            _indexOUT = _indexBusStop;
        }
        if (_indexBusStop == _indexOUT && _areDoorsOpen)
        {
            //тут система возрвщения на место но я ее непридумал когда двери на середине закрыты( почти придумал)
        }
        if (_indexBusStop == _indexOUT && !isWaiting && MoneyGive)
        {
            _Inbus = false;
            _Outbus = true;

        }
        if (_indexBusStop == _indexSpawn)
        {
            _Inbus = true;
            _Outbus = false;
        }
        if (_Inbus && _indexBusStop == _indexSpawn && _areDoorsOpen || _Inbus && transform.parent != null)
        {
            Gobus();
        }
        if (_Outbus && _isAtBusStop && _areDoorsOpen)
        {
            Outbus();
        }

        Debug.Log("_indexBusStop" + _indexBusStop);

    }
    private void AnimationSost()
    {
        if (target == null && !seat)
        {
            animator.Idle();
            return;
        }
        if (isWaiting)
        {
            animator.Idle();
            return;
        }
        else if (!seat && target != null || !_Inbus && !_Outbus)
        {
            Vector3 direction = new Vector3(target.position.x - transform.position.x, 0f, target.position.z - transform.position.z);

            if (direction.sqrMagnitude > 0.01f) // проверка на нулевое расстояние
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            animator.Walk();
            return;
        }
        if (seat) //!_Outbus && !_Inbus
        {
            animator.Sit();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, -180f + parentObject.eulerAngles.y, 0f), Time.deltaTime * rotationSpeed);
            return;
        }
    }
    private void LateUpdate()
    {
        _isAtBusStop = busController.s;
        _indexBusStop = busController.currentStopIndex;
    }
    private void Outbus()
    {
        seat = false;
        _Inbus = false;
        MoveToTargetExit();

    }
    private void Gobus()
    {
        _Outbus = false;
        if (!isWaiting && !seat)
        {
            MoveToTarget();
        }
        else if (isWaiting)
        {
            PayForRide();
        }
        if (seat)
        {
            Debug.Log("seat");
        }
    }
    public void FindWay()
    {
        Debug.Log("RowExit" + RowExit);
        if (RowExit == -2)
        {
            findWay.Way(index, out Transform targetPoint, out int inde, out int RowExitOut);
            index = inde;
            if (index == 2)
            {
                GetComponent<CapsuleCollider>().enabled = false;
                agent.enabled = false;
            }
            target = targetPoint;
            RowExit = RowExitOut;
        }
        else if (RowExit == -1)
        {
            findWay.SetSeatRowTarget(RowExit, index, out Transform targetPoint, out int inde, out int RowExitOut);
            target = targetPoint;
            index = inde;
            RowExit = RowExitOut;
        }
        else if (RowExit >= 0)
        {
            Debug.Log("иду сидеть");
            target = findWay.Seatpoints(index);
            goseat = true;

        }

    }
    private void MoveToTarget()
    {

        if (target == null)
        {
            Debug.Log("HUI");
            if (!seat)
            {
                FindWay();
            }

            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (index == stayIndex && !MoneyGive)
            {
                isWaiting = true; // Останавливаемся на точке ожидания
                Debug.Log($"Capsule reached stayIndex {stayIndex}. Waiting for trigger...");
            }
            else if (goseat)
            {
                seat = true;
            }
            else
            {
                FindWay();
            }
        }
    }

    public void FindWayOut()
    {
        Debug.Log("RowExit  на Входе " + RowExit);
        findWay.WayExit(RowExit, index, out Transform targetPoint, out int RowExitOut);
        RowExit = RowExitOut;
        target = targetPoint;
    }
    private void MoveToTargetExit()
    {
        if (target == null)
        {
            //findWay.ICanTMove();
            FindWayOut();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.03f)
        {
            if (RowExit == -2)
            {
                findWay.ICanMove();
                _Outbus = false;
                agent.enabled = true;
                GetComponent<CapsuleCollider>().enabled = true;
                return;
            }
            target = null;
            FindWayOut();
        }
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

        }


        DriverIncome.Instance.Money(billGiven, out isWaiting);
        if (!isWaiting)
        {
            Destroy(spawnedBill);
        }
        Debug.Log("isWaiting " + isWaiting);
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

        if (billPrefab != null && spawnPointMoneyPasajira != null)
        {
            // Спавним купюру и сохраняем ссылку на созданный объект
            spawnedBill = Instantiate(billPrefab, spawnPointMoneyPasajira.position, spawnPointMoneyPasajira.rotation);

            // Устанавливаем родителя для созданного объекта
            spawnedBill.transform.SetParent(parentObject.transform);
            spawnedBill.transform.localScale = new Vector3(20, 10, 20);  // Затем изменить масштаб

        }
        else
        {
            Debug.LogWarning("Пропущен спавн купюры: billPrefab или spawnPoint не определены.");
        }

    }

    public void ToggleDoor()
    {
        if (!_areDoorsOpen)
        {
            _areDoorsOpen = true;
        }
        else { _areDoorsOpen = false; }
    }

    private void Walk()
    {
        target = findWay.Gotarget(_indexOUT);
        agent.SetDestination(target.position);
        animator.Walk();
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ButtonDoor.OnButtonPressed -= ToggleDoor;
            Destroy(gameObject);
        }
    }
    public void SetIndex(int index, bool lastStop)
    {
        _indexSpawn = index;

        if (lastStop)
        {
            _indexOUT = 0;
        }
        else
        {
            _indexOUT = Random.Range(index + 1, 5);
        }
    }
}
