using UnityEngine;

public class CapsuleMover : MonoBehaviour
{
    public float speed = 2f; // �������� �������
    public int stayIndex = 0; // ������ �����, ��� ������� ���������������
    public KeyCode releaseKey = KeyCode.Return; // ������� ��� ������ �� ��������

    private Transform targetPoint; // ������� ����
    private Point[] points; // ������ �����
    private Point2[] points2; // ������ �����
    private Point3[] points3; // ������ �����
    private int currentIndex = 0; // ������� ������ �����
    private bool isWaiting = false; // ���� �������� �� �����
    private bool seat = false;
    private int RowExit = -1;

    private Animator animator;

    
    public bool AnimSeat = false;
    public bool Spisok1 = false;
    public bool MoneyGive = false;

    public Transform childObject;
    public Transform parentObject;// ������� �� ��������
    public GameObject[] billPrefabs; // ������ �������� ��� �����
    public Transform spawnPoint;
    private int ticketPrice = 30; // ��������� ������
    private int billGiven; // ������, ������� ��� ��������
    private BusStopTrigger busStopTrigger; // ��� �������� ��������� �� �������� �� ���������
    private BusController busController;
    public bool _isAtBusStop;
    public bool _areDoorsOpen;

    private void Start()
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

    private void Update()
    {
        if (AnimSeat)
        {
            bool isSitting = animator.GetBool("isSitting");
            animator.SetBool("isSitting", !isSitting);
            Debug.Log("�����");
            AnimSeat =false;

        }
        _isAtBusStop = busStopTrigger.isAtBusStop;
        _areDoorsOpen = busController.areDoorsOpen;       
        if (_isAtBusStop && _areDoorsOpen)
        {
            if (seat && RowExit != -1 && targetPoint == null)
            {
                Debug.Log("������� ��������� ��� ��������.");
                return; // ��������� Update
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
                    Debug.Log("�������� � 2 �������");
                }
                else
                {
                    MoveToTarget();
                }
            }
        }
    }


    /// <summary>
    /// ��������� ��������� ����� � �������� ����, ���� ��� ���� � �� ������.
    /// </summary>
    private void SetNextTarget()
    {

        if (currentIndex < points.Length && !points[currentIndex].IsOccupied)
        {
            targetPoint = points[currentIndex].transform; // ������������� ����
            points[currentIndex].Occupy(); // �������� �����
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
    /// ������� ������� � ������� ���� � ������������ ������ ���������� �����.
    /// </summary>
    private void MoveToTarget()
    {
        if (targetPoint == null) return;
        

        // �������� � ������� �����
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // �������� ���������� �����
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (seat && targetPoint == points3[RowExit].transform)
            {
                // ������� �������� ����� �������� ����
                Debug.Log($"������� �������� ����� �������� ����: RowExit = {RowExit}");
                targetPoint = null; // ���������� ����
                return; // ������������� ��������
            }

            if (currentIndex == stayIndex)
            {
                isWaiting = true; // ��������������� �� ����� ��������
                Debug.Log($"Capsule reached stayIndex {stayIndex}. Waiting for trigger...");
            }
            else
            {
                int nextIndex = currentIndex + 1;

                if (nextIndex < points.Length && !points[nextIndex].IsOccupied && !seat)
                {
                    points[currentIndex].Release(); // ����������� ������� �����
                    currentIndex = nextIndex; // ��������� � ���������
                    SetNextTarget(); // ������������� ����� ����
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

            // ��������� �����������, ��������������� ����
            Vector3 avoidanceDirection = -directionToTarget.normalized;

            // ��������� ���������� ��� ������ �����������
            Quaternion targetRotation = Quaternion.LookRotation(avoidanceDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private void SetSeatRowTarget()
    {
        points[currentIndex].Release();
        currentIndex = Random.Range(0, points2.Length);  // ����������� ��������� ��������, �������� ������ �������
        targetPoint = points2[currentIndex].transform;  // ������������� ����
        RowExit = currentIndex;  // ������������� RowExit
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
                Debug.Log("������������ currentIndex � SetSeatRowMumberTarget!" + currentIndex);
                return;
        }

        int attempts = 0;
        const int maxAttempts = 8;

        while (points3[RowExit].IsOccupied && attempts < maxAttempts)
        {
            RowExit = Random.Range(0, points3.Length); // ��������� ������ ��������
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("�� ������� ����� ��������� ����� � SetSeatRowMumberTarget.");
            return;
        }

        points3[RowExit].Occupy(); // �������� �����
        targetPoint = points3[RowExit].transform; // ������������� ����
        Debug.Log($"������� ������������ � ����� RowExit = {RowExit}");
        if (animator != null)
        {
            AnimSeat = true;
            Debug.Log("������ ����������");
        }
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
}
