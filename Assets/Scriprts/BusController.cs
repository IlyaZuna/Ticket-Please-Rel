using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // �������� ��������
    public float turnSpeed = 300f;   // �������� ��������
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public Transform frontLeftWheelModel;
    public Transform frontRightWheelModel;
    public Transform rearLeftWheelModel;
    public Transform rearRightWheelModel;
    public Transform exitpoint;
    [SerializeField] Transform ruder;

    private Rigidbody rb;
    public float maxSteerAngle = 30f;  // ������������ ���� �������� �����
    private float currentRotation = 0f;  // ������� ���� �������� ����
    public float turnSpeedudder = 100f;  // �������� �������� ����
    private float maxRotation = 440f;  // 2.5 ������� (360 * 2.5)
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
    }

    void Update()
    {
        // �������� ��������
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S ��� �������
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D ��� �������

        // ������������ ���� ��������
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle);

        // ������� ������� ������/�����
        rearLeftWheel.motorTorque = move * 300f;
        rearRightWheel.motorTorque = move * 300f;


        // ������� �����
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;
        // ��������� ������ �����
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
        UpdateCurrentStop();
    }
    // ��������� ������� � �������� �����
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
                Debug.Log("������� ������ �� ��������� � ��������: " + currentStopIndex);
                break; // ������������� ����, ��� ��� �� ����� ������� ���������
            }
            else
            {
                s = false;
                currentStopIndex = -1;
            }
        }

    }

}