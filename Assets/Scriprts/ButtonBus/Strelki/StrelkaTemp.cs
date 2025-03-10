using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrelkaTemp : MonoBehaviour
{
    [SerializeField] private BusController busController;
    private float minRotation = 65f;
    private float maxRotation = 125f;
    private float AngleConvenshion = 1.73f;
    private float currentRotation = 50f;
    public float engineTemperature = 50f;  // ��������� ����������� (�C)
    public float maxTemperature = 130f;    // ������������ ����������� (�C)
    public float coolingRate = 1f;         // �������� ���������� (�C/�)
    public float heatingRate = 0.5f;       // �������� ������� �� �������� ���� (�C/�)
    public float highSpeedHeating = 1.5f;  // �������������� ������ ��� �������� (�C/�)
    public float overheatingThreshold = 130f; // ����� ���������
    void Start()
    {
        busController = FindObjectOfType<BusController>();
    }
    void Update()
    {

        float speed = busController.GetSpeed();            

        // ������ ���������
        if (speed > 5)
        {
            engineTemperature += highSpeedHeating * (speed / 100f) * Time.deltaTime;
        }
        else
        {
            engineTemperature += heatingRate * Time.deltaTime; // ������ �� �������� ����
        }

        // ���������� (���� �������� ��������)
        if (speed < 20)
        {
            engineTemperature -= coolingRate * Time.deltaTime;
        }

        // ����������� �����������
        engineTemperature = Mathf.Clamp(engineTemperature, 50f, maxTemperature);


        currentRotation = (engineTemperature * AngleConvenshion);
        currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation); // ������������ ����
        transform.localRotation = Quaternion.Euler(-33, 0, currentRotation);

    }
}
