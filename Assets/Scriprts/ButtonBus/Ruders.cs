using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruders : MonoBehaviour
{
    public float turnSpeed = 300f;   // Скорость вращения руля
    public float returnSpeed = 5f;   // Скорость возврата в центр
    private float maxRotation = 900f; // 2.5 оборота (360 * 2.5)
    private float currentRotation = 0f; // Текущий угол поворота руля

    void Update()
    {
        float turnInput = Input.GetAxis("Horizontal"); // Получаем ввод от игрока

        if (Mathf.Abs(turnInput) > 0.01f)
        {
            // Если есть ввод, вращаем руль влево/вправо
            currentRotation += turnInput * turnSpeed * Time.deltaTime;
            currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation); // Ограничиваем угол
        }
        else
        {
            // Если клавиши не нажаты, плавно возвращаем руль к 0°
            currentRotation = Mathf.Lerp(currentRotation, 0f, returnSpeed * Time.deltaTime);
        }

        // Применяем вращение ТОЛЬКО к рулю по оси Z
        transform.localRotation = Quaternion.Euler(-75, 0, currentRotation);
    }
}

