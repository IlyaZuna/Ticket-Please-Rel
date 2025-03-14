using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassengerMove : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // �������� �������
    [SerializeField] private int stayIndex = 0; // ������ �����, ��� ������� ���������������
    private KeyCode releaseKey = KeyCode.Return; // ������� ��� ������ �� ��������

    private Transform targetPoint = null; // ������� ����
    private Point[] points; // ������ �����
    private Point2[] points2; // ������ �����
    private Point3[] points3; // ������ �����
    private int currentIndex = 0; // ������� ������ �����
    private int RowExit = -1;
    private int change = 0;
    public float rotationSpeed = 5f;
    public int driverChange;

    [SerializeField] private bool isWaiting = false; // ���� �������� �� �����
    [SerializeField] private bool seat = false;
    [SerializeField] private bool MoneyGive = false;
    [SerializeField] private bool _Inbus = true;
    [SerializeField] private bool _Outbus = false;
    [SerializeField] public AnimBase animator;
    [SerializeField] private Transform childObject;
    [SerializeField] private Transform parentObject;// ������� �� ��������
    [SerializeField] private GameObject[] billPrefabs; // ������ �������� ��� �����
    private GameObject billPrefab;
    private GameObject spawnedBill;
    [SerializeField] private Transform spawnPoint;

    private int billGiven; // ������, ������� ��� ��������
    private BusStopTrigger busStopTrigger; // ��� �������� ��������� �� �������� �� ���������
    private BusController busController;
    private FindWay findWay;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;
    private bool seattrue = false;
    private int _indexBusStop = -1;
    [SerializeField] private int _indexOUT;
    [SerializeField] private int _indexSpawn;
    [SerializeField] private int ticketPrice = 30; // ��������� ������
    [SerializeField] private Transform[] WalkPoint;
    private Transform target;
    private NavMeshAgent agent;
 
    private bool clear = false;
    void Start()
    {

        // ������� ��� ����� � ����� � ��������� �� �� �������
        points = FindObjectsOfType<Point>();
        System.Array.Sort(points, (a, b) => a.Index.CompareTo(b.Index));
        points2 = FindObjectsOfType<Point2>();
        System.Array.Sort(points2, (a, b) => a.Index.CompareTo(b.Index));
        points3 = FindObjectsOfType<Point3>();
        System.Array.Sort(points3, (a, b) => a.Index.CompareTo(b.Index));
        busStopTrigger = FindObjectOfType<BusStopTrigger>();
        busController = FindObjectOfType<BusController>();
        agent = GetComponent<NavMeshAgent>();

        
        if (WalkPoint != null && WalkPoint.Length != 0)
        {
            target = WalkPoint[_indexOUT];
        }
        ButtonDoor.OnButtonPressed += ToggleDoor;
    }

   
    void Update()
    {
        if (_indexOUT != -1)
        {
            //Debug.Log("!!!!!!!!!!!!!_indexSpawn" + _indexSpawn);
            //Debug.Log("!!!!!!!!!!!!!_indexSpawn" + _indexBusStop);
            if (_isAtBusStop && _areDoorsOpen)
            {
                if (_Inbus && _indexSpawn == _indexBusStop)
                {
                    
                        Gobus();
                    
                }
                if (_Outbus && _indexBusStop == _indexOUT)
                {

                    Outbus();
                }
            }
            if (targetPoint == null && !seattrue)
            {
                animator.Idle();
            }
            if (isWaiting)
            {
                animator.Idle();
            }
            else if (!seattrue && targetPoint != null)
            {
                Vector3 direction = new Vector3(targetPoint.position.x - transform.position.x, 0f, targetPoint.position.z - transform.position.z);


                if (direction.sqrMagnitude > 0.01f) // �������� �� ������� ����������
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                animator.Walk();
            }
            if (!_Outbus && !_Inbus)
            {
                seattrue = true;
                animator.Sit();
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, -180f + parentObject.eulerAngles.y, 0f), Time.deltaTime * rotationSpeed);
            }

            if (_indexBusStop != -1 && _indexBusStop == _indexOUT)
            {
                _Outbus = true;
            }
        }
        else
        {
            Walk();
        }

    }
    private void LateUpdate()
    {
        _isAtBusStop = busController.s;
        _indexBusStop = busController.currentStopIndex;
        //Debug.Log(_indexBusStop + "_indexBusStop");
        //_indexBusStop = busStopTrigger.indexStop;
    }
    private void Gobus()
    {       
      
            _Outbus = false;
            if (!isWaiting)
            {
                if (targetPoint == points[2].transform) { childObject.SetParent(parentObject); agent.enabled = false; }
                if (targetPoint == null && !seat && RowExit == -1) { SetNextTarget(); }
                if (seat && RowExit == -1) { SetSeatRowTarget(); }
                MoveToTarget();
            }
            else if (isWaiting)
            {
                PayForRide();
            }
        



    }   
    private void Outbus()
    {
        _Inbus = false;
        seattrue = false;
        seat = false;
        if (RowExit != -1 && !points2[RowExit].IsOccupied)
        {
            points2[RowExit].Occupy();
            targetPoint = points2[RowExit].transform;

        }


        MoveToTargetExit();

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
       // Debug.Log("������ �����������! �����: " + change);
       // driverChange = DriverIncome.Instance.GetChange();//������� �����

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (driverChange >= change)
            {
                //DriverIncome.Instance.AddIncome(ticketPrice);
                Debug.Log("�������� ������� �����: " + driverChange);
                //DriverIncome.Instance.GivepASAJChange(driverChange); // ������ ����� ���������                
                isWaiting = false;
                points[currentIndex].Release();
                Destroy(spawnedBill);
                seat = true;
                return;// ��������, ��� �������� �������
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

        if (billPrefab != null && spawnPoint != null)
        {
            // ������� ������ � ��������� ������ �� ��������� ������
            spawnedBill = Instantiate(billPrefab, spawnPoint.position, spawnPoint.rotation);

            // ������������� �������� ��� ���������� �������
            spawnedBill.transform.SetParent(parentObject.transform);
            spawnedBill.transform.localScale = new Vector3(20, 10, 20);  // ����� �������� �������

        }
        else
        {
            Debug.LogWarning("�������� ����� ������: billPrefab ��� spawnPoint �� ����������.");
        }

    }
    private void MoveToTarget()
    {

        if (targetPoint == null) { return; }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.03f)
        {
            if (currentIndex == stayIndex && !MoneyGive)
            {
                isWaiting = true; // ��������������� �� ����� ��������
                Debug.Log($"Capsule reached stayIndex {stayIndex}. Waiting for trigger...");
            }
            if (!seat && RowExit == -1 && !isWaiting)
            {
                SetNextTarget();
            }
            else if (seat && !isWaiting)
            {
                points2[RowExit].Release();
                targetPoint = points3[currentIndex].transform;
                seat = false;
            }
            else if (targetPoint == points3[currentIndex].transform)
            {

                Debug.Log("���");
                _Inbus = false;
                targetPoint = null;

                return;
            }


        }

    }
    private void SetNextTarget()
    {
        int nextindex = currentIndex + 1;
        if (nextindex == points.Length)
        {
            nextindex = currentIndex;
        }

        if (currentIndex < points.Length && !points[nextindex].IsOccupied)
        {

            points[currentIndex].Release(); // ����������� ������� �����
            targetPoint = points[nextindex].transform; // ������������� ����
            points[nextindex].Occupy(); // �������� �����

            currentIndex++;

        }
        else
        {
            targetPoint = null;
        }
    }
    private void SetSeatRowTarget()
    {
        targetPoint = null;
        currentIndex = Random.Range(0, 17);  // ����������� ��������� ��������, �������� ������ �������

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
                        Debug.Log("������������ currentIndex � SetSeatRowMumberTarget!" + currentIndex);
                        break;
                }
                points2[RowExit].Occupy();
                points3[currentIndex].Occupy();
                targetPoint = points2[RowExit].transform;  // ������������� ����
                Debug.Log("��������" + RowExit + "     currentIndex   " + currentIndex);
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
            Debug.Log("�� ��������");
        }


    }
    private void MoveToTargetExit()
    {
        if (targetPoint == null) { return; }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.03f)
        {
            if (RowExit != -1)
            {
                points2[RowExit].Release();
                RowExit = -1;
            }
            if (targetPoint == points[0].transform)
            {
                targetPoint = null;
                _Outbus = false;
                childObject.SetParent(null);
                agent.enabled = true;
                _indexOUT = -1;
                return;
            }
            if (targetPoint = points[0].transform)
            {
                if (points3[3].IsOccupied) { points[3].Release(); }
              
                points[0].Release();
                targetPoint = null;
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
        else if (!points[0].IsOccupied && targetPoint != points[3].transform && targetPoint != points[0].transform)
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
    private void Walk()
    {
        agent.SetDestination(target.position);
        animator.Walk();
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ButtonDoor.OnButtonPressed -= ToggleDoor;
            Destroy(gameObject);
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
    public void SetIndex(int index)
    {
        _indexSpawn = index;
        _indexOUT =Random.Range(index, 6);
    }
   
}

