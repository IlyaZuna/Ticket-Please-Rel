using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayTest : MonoBehaviour
{
    private Transform target;
    private FindWay findWay;
    private int index = 0;
    private float speed = 5f;
    private int RowExit = -2;
    public float rotationSpeed = 5f;
    [SerializeField] private int stayIndex = 4;
    [SerializeField] private bool isWaiting = false;
    [SerializeField] private bool MoneyGive = false;
    [SerializeField] private bool seat = false;
    [SerializeField] private bool goseat = false;
    [SerializeField] public AnimBase animator; 


    [SerializeField] private GameObject[] billPrefabs; // ������ �������� ��� �����
    [SerializeField] private Transform spawnPointMoneyPasajira; 
    private GameObject billPrefab;
    private GameObject spawnedBill;   
    private int billGiven; // ������, ������� ��� ��������



    private int change = 0;
    [SerializeField] private int ticketPrice = 30;
    public int driverChange;


    
    [SerializeField] private Transform childObject;
    [SerializeField] private Transform parentObject;// ������� �� ��������


    [SerializeField] private bool _Inbus = true;
    [SerializeField] private bool _Outbus = false;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;
    private BusController busController;

    private int _indexBusStop = -1;
    [SerializeField] private int _indexOUT;
    [SerializeField] private int _indexSpawn;


    [SerializeField] private Transform[] WalkPoint;
    private Transform targetWalk;

    void Start()
    {        
        busController = FindObjectOfType<BusController>();
        findWay = FindObjectOfType<FindWay>();
        if (WalkPoint != null && WalkPoint.Length != 0)
        {
            targetWalk = WalkPoint[_indexOUT];
        }
        ButtonDoor.OnButtonPressed += ToggleDoor;
    }
    void Update()
    {   if(_indexBusStop > _indexOUT)
        {
            _indexOUT = _indexBusStop;
        }
        if (_indexBusStop == _indexOUT && _areDoorsOpen)
        {
            //��� ������� ���������� �� ����� �� � �� ���������� ����� ����� �� �������� �������( ����� ��������)
        }
        if (_indexBusStop == _indexOUT)
        {
            _Inbus = false;
            _Outbus = true;
            findWay.ICanTMove();
        }
        if (_indexBusStop == _indexSpawn)
        {
            _Inbus = true;
            _Outbus = false;
        }
        if (_Inbus && _isAtBusStop && _areDoorsOpen || _Inbus && transform.parent != null)
        {
            Gobus();
        }
        if (_Outbus && _isAtBusStop && _areDoorsOpen)
        {
            Outbus();
        }
        AnimationSost();


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
        else if (!seat && target != null)
        {
            Vector3 direction = new Vector3(target.position.x - transform.position.x, 0f, target.position.z - transform.position.z);

            if (direction.sqrMagnitude > 0.01f) // �������� �� ������� ����������
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
        seat=false;
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
        Debug.Log("RowExit"+ RowExit);
        if (RowExit == -2)
        {
            findWay.Way(index, out Transform targetPoint, out int inde, out int RowExitOut);           
            index = inde;
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
        else if(RowExit >= 0)
        {
            Debug.Log("��� ������");
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
        if (Vector3.Distance(transform.position, target.position) < 0.03f)
        {
            if (index == stayIndex && !MoneyGive)
            {
                isWaiting = true; // ��������������� �� ����� ��������
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
        Debug.Log("RowExit  �� ����� " + RowExit);
        findWay.WayExit(RowExit,index, out Transform targetPoint, out int RowExitOut);
        RowExit = RowExitOut;
        target = targetPoint;
    }
    private void MoveToTargetExit()
    {
        if (target == null)
        {
            FindWayOut();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.03f)
        {            
            if(RowExit == -2)
            {
                findWay.ICanMove();
                return;
            }
            target = null;
            FindWayOut();
        }
    }

    private void PayForRide()
    {
        // ��������� ������� �������� ������ (50, 100 ��� 200)
        if (MoneyGive == false)
        {
            billGiven = GetRandomBill();
            MoneyGive = true;
            SpawnBill(billGiven);
            Debug.Log("�������� ��� ������: " + billGiven);
            change = billGiven - ticketPrice; // ������������ �����
        }
        Debug.Log("������ �����������! �����: " + change);
        driverChange = DriverIncome.Instance.GetChange();//������� �����

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (driverChange >= change)
            {
                //DriverIncome.Instance.AddIncome(ticketPrice);
                Debug.Log("�������� ������� �����: " + driverChange);
                DriverIncome.Instance.GivepASAJChange(driverChange); // ������ ����� ���������                
                isWaiting = false;                
                Destroy(spawnedBill);               
                return;
            }
            else
            {
                Debug.Log("����");
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


        // ����������, ����� ������ ������� �� ������ �������� ������
        switch (billAmount)
        {

            case 50:
                billPrefab = billPrefabs[0]; // ������ ��� ������ 50
                break;
            case 100:
                billPrefab = billPrefabs[1]; // ������ ��� ������ 100
                break;
            case 200:
                billPrefab = billPrefabs[2]; // ������ ��� ������ 200
                break;
            default:
                break;
        }

        if (billPrefab != null && spawnPointMoneyPasajira != null)
        {
            // ������� ������ � ��������� ������ �� ��������� ������
            spawnedBill = Instantiate(billPrefab, spawnPointMoneyPasajira.position, spawnPointMoneyPasajira.rotation);

            // ������������� �������� ��� ���������� �������
            spawnedBill.transform.SetParent(parentObject.transform);
            spawnedBill.transform.localScale = new Vector3(20, 10, 20);  // ����� �������� �������

        }
        else
        {
            Debug.LogWarning("�������� ����� ������: billPrefab ��� spawnPoint �� ����������.");
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

}
