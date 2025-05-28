using UnityEngine;
using System.Collections;

public class KeyON : MonoBehaviour, IInteractable
{
    [SerializeField] private BusController busController;
    [SerializeField] private ManagerBus manager;
    [SerializeField] private GameObject ignitionKey; // Ссылка на ключ в сцене
    [SerializeField] private Transform keyInsertPoint; // Точка, куда вставляется ключ
    [SerializeField] private float rotationSpeed = 2f;  // Скорость поворота
    [SerializeField] private float insertSpeed = 2f;    // Скорость анимации вставки/извлечения
    [SerializeField] private Vector3 insertOffset = new Vector3(0, 0, 0.1f); // Смещение для анимации вставки

    public IgnitionState currentIgnitionState = IgnitionState.Removed; // Начальное состояние
    private Quaternion targetRotation;         // Целевая ротация ключа
    private bool isAnimating = false;          // Флаг анимации (вставка, поворот, извлечение)
    private Vector3 insertedPosition;          // Позиция ключа после вставки
    private Vector3 removedPosition;           // Позиция ключа перед вставкой/после извлечения

    public enum IgnitionState
    {
        Removed,   // 0 - Ключ убран
        Inserted,  // 1 - Ключ вставлен (0 градусов)
        PowerMode, // 2 - Вспомогательное питание (-45 градусов)
        Start      // 3 - Запуск двигателя (-90 градусов)
    }

    void Start()
    {
        busController = FindObjectOfType<BusController>();
        if (keyInsertPoint == null)
        {
            Debug.LogError("Key Insert Point is not assigned in the Inspector!");
        }
        if (ignitionKey == null)
        {
            Debug.LogError("Ignition Key is not assigned in the Inspector!");
        }
        else
        {
            // Устанавливаем начальные параметры
            ignitionKey.SetActive(false); // Ключ изначально невидим
            ignitionKey.transform.SetParent(keyInsertPoint); // Привязываем ключ к точке вставки
            insertedPosition = keyInsertPoint.position; // Позиция вставленного ключа
            removedPosition = insertedPosition + insertOffset; // Позиция перед вставкой/после извлечения
            ignitionKey.transform.position = removedPosition; // Начальная позиция
            ignitionKey.transform.localRotation = Quaternion.Euler(0, 0, 0); // Начальная ротация
        }
    }

    public void Interact()
    {
        if (isAnimating || ignitionKey == null) return;

        switch (currentIgnitionState)
        {
            case IgnitionState.Removed:
                currentIgnitionState = IgnitionState.Inserted;
                targetRotation = Quaternion.Euler(0, 0, 0); // Начальная ротация (0 градусов)
                StartCoroutine(InsertKey());
                break;

            case IgnitionState.Inserted:
                currentIgnitionState = IgnitionState.PowerMode;
                targetRotation = Quaternion.Euler(0, 45, 0); // Поворот против часовой на -45 градусов
                StartCoroutine(RotateKey());
                Off();
                PowerMode();
                break;

            case IgnitionState.PowerMode:
                currentIgnitionState = IgnitionState.Start;
                targetRotation = Quaternion.Euler(0, 90, 0); // Поворот против часовой на -90 градусов
                StartCoroutine(RotateKey());
                StartEngine();
                break;

            case IgnitionState.Start:
                currentIgnitionState = IgnitionState.Removed;
                targetRotation = Quaternion.Euler(0, 0, 0);
                StartCoroutine(RotateKey());
                StartCoroutine(RemoveKey());
                Off();
                break;
        }
    }
    public void LateUpdate()
    {
        if (keyInsertPoint == null)
        {
            Debug.LogError("Key Insert Point is not assigned in the Inspector!");
        }
        if (ignitionKey == null)
        {
            Debug.LogError("Ignition Key is not assigned in the Inspector!");
        }
        else
        {
            insertedPosition = keyInsertPoint.position; // Позиция вставленного ключа
            removedPosition = insertedPosition + insertOffset; // Позиция перед вставкой/после извлечения
            ignitionKey.transform.position = removedPosition; // Начальная позиция

        }
    }
    private IEnumerator InsertKey()
    {
        isAnimating = true;
        ignitionKey.SetActive(true); // Показываем ключ

        Vector3 startPosition = removedPosition;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * insertSpeed;
            ignitionKey.transform.position = Vector3.Lerp(startPosition, insertedPosition, time);
            yield return null;
        }
        ignitionKey.transform.position = insertedPosition;
        isAnimating = false;
    }

    private IEnumerator RotateKey()
    {
        isAnimating = true;
        Quaternion startRotation = ignitionKey.transform.localRotation;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            ignitionKey.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time);
            yield return null;
        }

        ignitionKey.transform.localRotation = targetRotation; // Убеждаемся, что ротация точная
        isAnimating = false;
    }

    private IEnumerator RemoveKey()
    {
        isAnimating = true;
        Vector3 startPosition = insertedPosition;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * insertSpeed;
            ignitionKey.transform.position = Vector3.Lerp(startPosition, removedPosition, time);
            yield return null;
        }
        ignitionKey.transform.position = removedPosition;
        ignitionKey.SetActive(false); // Убираем ключ
        isAnimating = false;
    }

    void Off()
    {
        manager.StepOne(false);
        manager.StepTwo(false);
    }

    void PowerMode()
    {
        manager.StepOne(true);
    }

    void StartEngine()
    {
        manager.StepTwo(true);
    }
}