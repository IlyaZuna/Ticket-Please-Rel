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
    public int currentStopIndex = -1; // Индекс текущей остановки

    private Rigidbody rb;
    public float maxSteerAngle = 30f;  // Максимальный угол поворота колес
    public bool isDriver = false;

    // Новые переменные для состояния дверей и остановки
    public bool areDoorsOpen = false;  // Состояние дверей (открыты/закрыты)
    public bool s = false;      // Находится ли автобус на остановке

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Получаем Rigidbody автобуса
    }

    void Update()
    {
        // Проверяем, нажата ли клавиша "1" и находится ли автобус на остановке
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleDoors();  // Вызов метода переключения состояния дверей
        }

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
        if (currentStopIndex != -1)
        {
            Debug.Log("Текущая остановка: " + currentStopIndex);
        }
    }

    // Метод для переключения состояния дверей...........................................................................................................................................................................................................
    void ToggleDoors()
    {
        areDoorsOpen = !areDoorsOpen;// Меняем состояние дверей
        Debug.Log("Двери " + (areDoorsOpen ? "открыты" : "закрыты"));
    }
    //.....................................................................................................................................................................................................................................
    // Обновляем позиции и вращения колес
    void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelModel)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelModel.position = pos;
        wheelModel.rotation = rot;
    }

     

}