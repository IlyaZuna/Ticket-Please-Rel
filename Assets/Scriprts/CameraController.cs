using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Чувствительность мыши
    public Transform playerBody;           // Ссылка на автобус (корпус)

    private float xRotation = 0f;          // Хранит поворот по оси X (вверх и вниз)
    private float yRotation = 180f;        // Устанавливаем начальный поворот по оси Y на 180 градусов

    void Start()
    {
        // Блокируем курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;

        // Устанавливаем начальный поворот камеры
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Update()
    {
        // Получаем движение мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Инвертируем ось Y для корректного поворота камеры вверх и вниз
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);  // Ограничение угла поворота камеры вверх и вниз

        // Поворот по оси Y (влево и вправо)
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, 120f, 260f);  // Ограничение угла поворота камеры влево и вправо

        // Применяем повороты к камере
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
