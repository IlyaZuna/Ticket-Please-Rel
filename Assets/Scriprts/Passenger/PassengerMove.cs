using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerMove : MonoBehaviour
{
    [SerializeField]private float speed = 2f; // �������� �������
    [SerializeField] private int stayIndex = 0; // ������ �����, ��� ������� ���������������
    private KeyCode releaseKey = KeyCode.Return; // ������� ��� ������ �� ��������

    private Transform targetPoint; // ������� ����
    private Point[] points; // ������ �����
    private Point2[] points2; // ������ �����
    private Point3[] points3; // ������ �����
    private int currentIndex = 0; // ������� ������ �����
    private int RowExit = -1;

    private Animator animator;

    private bool isWaiting = false; // ���� �������� �� �����
    private bool seat = false;
    private bool AnimSeat = false;
    private bool Spisok1 = false;
    private bool MoneyGive = false;
    [SerializeField] private bool _Inbus = true;
    [SerializeField] private bool _Outbus = false;

    private Transform childObject;
    private Transform parentObject;// ������� �� ��������
    private GameObject[] billPrefabs; // ������ �������� ��� �����
    private Transform spawnPoint;
    private int ticketPrice = 30; // ��������� ������
    private int billGiven; // ������, ������� ��� ��������
    private BusStopTrigger busStopTrigger; // ��� �������� ��������� �� �������� �� ���������
    private BusController busController;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;
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
                Debug.Log("�����");
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
        // ��������� ������� �������� ������ (50, 100 ��� 200)
        if (MoneyGive == false)
        {
            billGiven = GetRandomBill();
            MoneyGive = true;
            DriverIncome.Instance.AddIncome(ticketPrice); // ��������� ����� ��������
            SpawnBill(billGiven);
            Debug.Log("�������� ��� ������: " + billGiven);
        }
        else
        {
            int change = billGiven - ticketPrice; // ������������ �����
            Debug.Log("������ �����������! �����: " + change);

            // �������� ����� �� ��������
            int driverChange = DriverIncome.Instance.GetChange();
            if (Input.GetKeyDown(KeyCode.Q) && driverChange > 0)
            {
                Debug.Log("�������� ������� �����: " + driverChange);
                DriverIncome.Instance.GiveChange(driverChange); // ������ ����� ���������
                Spisok1 = true;
                MoneyGive = false;
                return;// ��������, ��� �������� �������
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
        }

        if (billPrefab != null && spawnPoint != null)
        {
            // ������� ������ � ��������� ������ �� ��������� ������
            GameObject spawnedBill = Instantiate(billPrefab, spawnPoint.position, spawnPoint.rotation);

            // ������������� �������� ��� ���������� �������
            spawnedBill.transform.SetParent(parentObject.transform);

            Debug.Log("������ ��������: " + billAmount);
        }
        else
        {
            Debug.LogWarning("�������� ����� ������: billPrefab ��� spawnPoint �� ����������.");
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
                points[currentIndex].Release(); // ����������� ������� �����
                currentIndex++;
                SetNextTarget();
            }
            else if (currentIndex == stayIndex)
            {
                isWaiting = true; // ��������������� �� ����� ��������
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
                Debug.Log("���");
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
            targetPoint = points[currentIndex].transform; // ������������� ����
            points[currentIndex].Occupy(); // �������� �����         
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
                Debug.Log("��������"+RowExit+ "     currentIndex   " + currentIndex);
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
