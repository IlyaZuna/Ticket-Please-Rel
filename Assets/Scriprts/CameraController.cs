using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Чувствительность мыши
    public Transform playerBody;           // Ссылка на автобус (корпус)

    private float xRotation = 0f;          // Хранит поворот по оси X (вверх и вниз)
    private float yRotation = 180f;        // Устанавливаем начальный поворот по оси Y на 180 градусов
    [SerializeField] private Camera playerCamera; // Камера игрока
    [SerializeField] private float interactionDistance = 5f; // Дистанция взаимодействия

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
        RayCaster();
    }
    private void RayCaster()
    {
        Vector3 rayOrigin = playerCamera.transform.position;

        // Направление луча. Это может быть любое направление от камеры, например, вперед по оси Z
        Vector3 rayDirection = playerCamera.transform.forward;  // Направление вперед от камеры

        // Создаем рэй из камеры
        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus");

        RaycastHit hit;

        // Выполняем рэйкаст с ограничением дистанции
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // Проверяем, попал ли луч в объект
            Debug.Log($"Рэй попал в объект: {hit.collider.name}");

            // Проверяем, есть ли компонент BillEtMoney на объекте, в который попал луч
            var bill = hit.collider.GetComponent<BiilllEtMoney>();
            if (bill != null)
            {
                // Если компонент найден, делаем что-то с этим объектом
                Debug.Log("Попали в BillEtMoney объект!");
                // Здесь можно вызвать метод, например, OnMouseDown()
                bill.OnMouseDown(); // Вызов метода из BillEtMoney компонента
            }
        }



    }
}
