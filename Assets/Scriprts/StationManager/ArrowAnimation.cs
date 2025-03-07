using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public float rotationSpeed = 30f; // Скорость вращения (градусов в секунду)
    public float floatAmplitude = 0.1f; // Амплитуда колебания по высоте
    public float floatSpeed = 2f; // Скорость колебания

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; // Сохраняем начальную позицию
    }

    void Update()
    {
        // Вращение вокруг оси Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Колебание по высоте
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
