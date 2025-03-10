using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dworn : MonoBehaviour
{
    public float speed = 100f; // Скорость вращения
    public float minAngle = -30f; // Минимальный угол наклона
    public float maxAngle = 30f; // Максимальный угол наклона
    [SerializeField]private float currentAngle;
    private float StartPoz;
    private bool movingForward = true;
    private bool ON = false;

    private void Start()
    {
        ButtonDworniki.OnButtonPressed += ONN;
        StartPoz = currentAngle;
    }
    void Update()
    {
        if (ON)
        {
            RotateWipers();
        }
        else
        {
            GoStartPoz();
        }
    }
    public void ONN( )
    {
        ON = !ON;
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
    private void GoStartPoz()
    {
        if (currentAngle != StartPoz)
        {
            RotateWipers();           
        }
        else
        {
            return;
        }
    }
}