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

    void LateUpdate()
    {
        // Получаем движение мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Инвертируем ось Y для корректного поворота камеры вверх и вниз
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 35f);  // Ограничение угла поворота камеры вверх и вниз

        // Поворот по оси Y (влево и вправо)
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, 130f, 340f);  // Ограничение угла поворота камеры влево и вправо

        // Применяем повороты к камере
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        RayCaster();
    }
    private void RayCaster()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward; // Направление вперед от камеры

        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus"); // Игнорируем слой "Bus"

        RaycastHit hit;

        // Список тегов, которые нужно игнорировать
        string[] ignoredTags = { "BusStop" };

        // Выполняем рэйкаст с ограничением дистанции
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // Проверяем, попал ли луч в объект с игнорируемым тегом
            foreach (string tag in ignoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    return; // Выходим из метода, если объект имеет игнорируемый тег
                }
            }

            Debug.Log($"Рэй попал в объект: {hit.collider.name}");

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (Input.GetMouseButtonDown(0) && interactable != null)
            {
                interactable.Interact();
            }

            // Проверяем, есть ли компонент BillEtMoney
            var bill = hit.collider.GetComponent<BiilllEtMoney>();
            if (bill != null)
            {
                Debug.Log("Попали в BillEtMoney объект!");
                bill.OnMouseDown();
            }
        }
    }
}
