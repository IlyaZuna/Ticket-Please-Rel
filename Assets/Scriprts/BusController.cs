using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // Скорость движения
    public float turnSpeed = 300f;   // Скорость поворота
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
    public float maxSteerAngle = 30f;  // Максимальный угол поворота колес
    private float currentRotation = 0f;  // Текущий угол поворота руля
    public float turnSpeedudder = 100f;  // Скорость вращения руля
    private float maxRotation = 440f;  // 2.5 оборота (360 * 2.5)
    public bool isDriver = false;

    // Новые переменные для состояния дверей и остановки
    public bool areDoorsOpen = false;  // Состояние дверей (открыты/закрыты)
    public bool s = false;      // Находится ли автобус на остановке
    public int currentStopIndex = -1; // Индекс текущей остановки
    private BusStopTrigger[] stops; // Массив всех остановок на сцене

    void Start()
    {
        stops = FindObjectsOfType<BusStopTrigger>();
        rb = GetComponent<Rigidbody>(); // Получаем Rigidbody автобуса
    }

    void Update()
    {
        // Движение автобуса
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S или стрелки
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D или стрелки

        // Ограничиваем угол поворота
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle);

        // Двигаем автобус вперед/назад
        rearLeftWheel.motorTorque = move * 300f;
        rearRightWheel.motorTorque = move * 300f;


        // Поворот колес
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;
        // Обновляем модели колес
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
        UpdateCurrentStop();
    }
    // Обновляем позиции и вращения колес
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
            if (stop.isAtBusStop) // Если автобус на этой остановке
            {
                s = true;
                currentStopIndex = stop.indexStop; // Сохраняем индекс этой остановки
                Debug.Log("Автобус сейчас на остановке с индексом: " + currentStopIndex);
                break; // Останавливаем цикл, так как мы нашли текущую остановку
            }
            else
            {
                s = false;
                currentStopIndex = -1;
            }
        }

    }

}