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
    public float engineTemperature = 50f;  // Начальная температура (°C)
    public float maxTemperature = 130f;    // Максимальная температура (°C)
    public float coolingRate = 1f;         // Скорость охлаждения (°C/с)
    public float heatingRate = 0.5f;       // Скорость нагрева на холостом ходу (°C/с)
    public float highSpeedHeating = 1.5f;  // Дополнительный нагрев при движении (°C/с)
    public float overheatingThreshold = 130f; // Порог перегрева
    void Start()
    {
        busController = FindObjectOfType<BusController>();
    }
    void Update()
    {

        float speed = busController.GetSpeed();            

        // Нагрев двигателя
        if (speed > 5)
        {
            engineTemperature += highSpeedHeating * (speed / 100f) * Time.deltaTime;
        }
        else
        {
            engineTemperature += heatingRate * Time.deltaTime; // Нагрев на холостом ходу
        }

        // Охлаждение (если движется медленно)
        if (speed < 20)
        {
            engineTemperature -= coolingRate * Time.deltaTime;
        }

        // Ограничение температуры
        engineTemperature = Mathf.Clamp(engineTemperature, 50f, maxTemperature);


        currentRotation = (engineTemperature * AngleConvenshion);
        currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation); // Ограничиваем угол
        transform.localRotation = Quaternion.Euler(-33, 0, currentRotation);

    }
}
