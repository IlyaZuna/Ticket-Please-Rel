using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float mouseSensitivity = 100f;  // Чувствительность мыши
    public Transform playerBody;           // Ссылка на автобус (корпус)

    private float xRotation = 0f;          // Хранит поворот по оси X (вверх и вниз)
    private float yRotation = 180f;        // Устанавливаем начальный поворот по оси Y на 180 градусов
    [SerializeField] private Camera playerCamera; // Камера игрока
    [SerializeField] private float interactionDistance = 5f; // Дистанция взаимодействия

    // Переменные для подсветки
    private Material highlightMaterial;    // Материал для подсветки
    private Renderer lastHighlightedRenderer; // Последний подсвеченный рендерер
    private Material originalMaterial;     // Оригинальный материал объекта
    [SerializeField] private Color outlineColor = Color.yellow; // Цвет подсветки
    [SerializeField] private float outlineWidth = 0.03f; // Ширина контура

    // Список тегов, которые Raycast должен игнорировать
    [SerializeField] private string[] raycastIgnoredTags = { "RaycastIgnore" };
    // Список тегов, которые нужно игнорировать для взаимодействия
    [SerializeField] private string[] ignoredTags = { "BusStop" };

    void Start() {
        // Блокируем курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;

        // Устанавливаем начальный поворот камеры
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Инициализация материала для подсветки
        highlightMaterial = new Material(Shader.Find("Custom/HighlightShader"));
        highlightMaterial.SetColor("_OutlineColor", outlineColor);
        highlightMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }
    private void Update()
    {
        RayCaster();
    }
    void LateUpdate() {
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
        
    }

    private void RayCaster() {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward; // Направление вперед от камеры

        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus"); // Игнорируем слой "Bus"

        RaycastHit hit;

        // Визуализация луча для отладки
        Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, Color.red, 1f);

        // Выполняем рэйкаст с ограничением дистанции
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // Проверяем, попал ли луч в объект с тегом, который игнорируется Raycast'ом
            foreach (string tag in raycastIgnoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    ResetHighlight(); // Сбрасываем подсветку
                    return; // Выходим из метода
                }
            }

            // Проверяем, попал ли луч в объект с игнорируемым тегом для взаимодействия
            foreach (string tag in ignoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    ResetHighlight();
                    return;
                }
            }

            Renderer currentRenderer = hit.collider.GetComponent<Renderer>();

            // Логика подсветки
            if (currentRenderer != null)
            {
                if (currentRenderer != lastHighlightedRenderer)
                {
                    ResetHighlight(); // Сбрасываем предыдущую подсветку
                    originalMaterial = currentRenderer.material; // Сохраняем оригинальный материал
                    currentRenderer.material = highlightMaterial; // Применяем подсветку
                    lastHighlightedRenderer = currentRenderer;
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
        else
        {
            // Если луч ни во что не попал, сбрасываем подсветку
            ResetHighlight();
        }
    }

    private void ResetHighlight() {
        if (lastHighlightedRenderer != null)
        {
            lastHighlightedRenderer.material = originalMaterial;
            lastHighlightedRenderer = null;
        }
    }

    void OnDestroy() {
        // Очищаем созданный материал при уничтожении объекта
        if (highlightMaterial != null)
        {
            Destroy(highlightMaterial);
        }
    }
}