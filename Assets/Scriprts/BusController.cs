using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // �������� ��������
    public float turnSpeed = 300f;   // �������� ��������
    [SerializeField] public float maxFuel = 100f;  // ������������ ����� �������
    public float currentFuel;     // ������� ���������� �������
    public float fuelConsumptionRate = 0.1f; // ������ ������� (�/� ��� ������������ ��������)
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public Transform frontLeftWheelModel;
    public Transform frontRightWheelModel;
    public Transform rearLeftWheelModel;
    public Transform rearRightWheelModel;
    public Transform exitpoint;
    public bool ON =false;
    [SerializeField] Transform ruder;

    public float brakeDrag = 2f; // ������������� ��� ����������
    private float normalDrag; // ������� �������������

    private Rigidbody rb;
    public float maxSteerAngle = 30f;  // ������������ ���� �������� �����    
    public float turnSpeedudder = 100f;  // �������� �������� ����   
    public bool isDriver = false;

    // ����� ���������� ��� ��������� ������ � ���������
    public bool areDoorsOpen = false;  // ��������� ������ (�������/�������)
    public bool s = false;      // ��������� �� ������� �� ���������
    public int currentStopIndex = -1; // ������ ������� ���������
    private BusStopTrigger[] stops; // ������ ���� ��������� �� �����

    void Start()
    {
        stops = FindObjectsOfType<BusStopTrigger>();
        rb = GetComponent<Rigidbody>(); // �������� Rigidbody ��������
        normalDrag = rb.drag; // ���������� ����������� �������������
        currentFuel = maxFuel;
    }

    void Update()
    {
        if (ON)
        {
            MoveBus();
        } 
    }
    public void Engine(bool state)
    {
        ON = state;
    }
    void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelModel)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelModel.position = pos;
        wheelModel.rotation = rot;
    }

    public void UpdateCurrentStop()
    {
        foreach (var stop in stops)
        {
            if (stop.isAtBusStop) // ���� ������� �� ���� ���������
            {
                s = true;
                currentStopIndex = stop.indexStop; // ��������� ������ ���� ���������
                //Debug.Log("������� ������ �� ��������� � ��������: " + currentStopIndex);
                break; // ������������� ����, ��� ��� �� ����� ������� ���������
            }
            else
            {
                s = false;
                currentStopIndex = -1;
            }
        }

    }
    public void SetBraking(bool isBraking)
    {
        rb.drag = isBraking ? brakeDrag : normalDrag;
    }
    public float GetSpeed()
    {
        float speed = rb.velocity.magnitude * 3.6f; ;        
        return speed;
    }
    public float GetFuil()
    {
        
        return currentFuel;
    }

    public void FuilOut()
    {
        float speed = rb.velocity.magnitude * 3.6f; // �������� � ��/�
        float fuelConsumption = fuelConsumptionRate * (speed / 100f); // ������ ������� �� ��������
        currentFuel -= fuelConsumption * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel); // ����� �� ������� � �����
    }

    public void MoveBus()
    {
        // �������� ��������
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S ��� �������
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D ��� �������

        if (currentFuel <= 0)
        {
            move = 0; // ������������� �������
            Debug.Log("������� �����������! �����������!");
        }
        // ������������ ���� ��������
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle);

        // ������� ������� ������/�����
        rearLeftWheel.motorTorque = move * 300f;
        rearRightWheel.motorTorque = move * 300f;
        //Debug.Log("�������� ��������: " + GetSpeed() + " �/�");


        // ������� �����
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;
        // ��������� ������ �����
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
        UpdateCurrentStop();
        // ���������� �� ������
        if (Input.GetKey(KeyCode.Space))
        {
            SetBraking(true);
        }
        else
        {
            SetBraking(false);
        }
        FuilOut();

    }
}