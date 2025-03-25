using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BusController : MonoBehaviour
{
    public float moveSpeed = 500f;   // Скорость движения
    public float turnSpeed = 300f;   // Скорость поворота
    [SerializeField] public float maxFuel = 100f;  // Максимальный объем топлива
    public float currentFuel;     // Текущее количество топлива
    public float fuelConsumptionRate = 0.1f; // Расход топлива (л/с при максимальной нагрузке)
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

    public float brakeDrag = 2f; // Сопротивление при торможении
    private float normalDrag; // Обычное сопротивление

    private Rigidbody rb;
    public float maxSteerAngle = 30f;  // Максимальный угол поворота колес    
    public float turnSpeedudder = 100f;  // Скорость вращения руля   
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
        normalDrag = rb.drag; // Запоминаем стандартное сопротивление
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
            if (stop.isAtBusStop) // Если автобус на этой остановке
            {
                s = true;
                currentStopIndex = stop.indexStop; // Сохраняем индекс этой остановки
                //Debug.Log("Автобус сейчас на остановке с индексом: " + currentStopIndex);
                break; // Останавливаем цикл, так как мы нашли текущую остановку
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
        float speed = rb.velocity.magnitude * 3.6f; // Скорость в км/ч
        float fuelConsumption = fuelConsumptionRate * (speed / 100f); // Расход зависит от скорости
        currentFuel -= fuelConsumption * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel); // Чтобы не уходило в минус
    }

    public void MoveBus()
    {
        // Движение автобуса
        float move = -Input.GetAxis("Vertical") * moveSpeed; // W/S или стрелки
        float turn = Input.GetAxis("Horizontal") * turnSpeed; // A/D или стрелки

        if (currentFuel <= 0)
        {
            move = 0; // Останавливаем автобус
            Debug.Log("Топливо закончилось! Заправьтесь!");
        }
        // Ограничиваем угол поворота
        turn = Mathf.Clamp(turn, -maxSteerAngle, maxSteerAngle);

        // Двигаем автобус вперед/назад
        rearLeftWheel.motorTorque = move * 300f;
        rearRightWheel.motorTorque = move * 300f;
        //Debug.Log("Скорость автобуса: " + GetSpeed() + " м/с");


        // Поворот колес
        frontLeftWheel.steerAngle = turn;
        frontRightWheel.steerAngle = turn;
        // Обновляем модели колес
        UpdateWheelPosition(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelPosition(frontRightWheel, frontRightWheelModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelPosition(rearRightWheel, rearRightWheelModel);
        UpdateCurrentStop();
        // Торможение на пробел
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