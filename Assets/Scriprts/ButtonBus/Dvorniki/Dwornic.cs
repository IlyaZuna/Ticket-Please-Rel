using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dworn : MonoBehaviour
{
    public float speed = 100f; // Скорость вращения
    public float minAngle = -30f; // Минимальный угол наклона
    public float maxAngle = 30f; // Максимальный угол наклона
    [SerializeField]private float currentAngle;
    private bool movingForward = true;

    void Update()
    {
        RotateWipers();
    }

    void RotateWipers()
    {
        // Вычисляем угол поворота
        if (movingForward)
        {
            currentAngle += speed * Time.deltaTime;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                movingForward = false;
            }
        }
        else
        {
            currentAngle -= speed * Time.deltaTime;
            if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;
                movingForward = true;
            }
        }

        // Применяем поворот
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, currentAngle, transform.localRotation.eulerAngles.z);
    }
}