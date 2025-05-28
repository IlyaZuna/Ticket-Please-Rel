using UnityEngine;
using System.Collections;

public class KeyON : MonoBehaviour, IInteractable
{
    [SerializeField] private BusController busController;
    [SerializeField] private ManagerBus manager;
    [SerializeField] private GameObject ignitionKey; // ������ �� ���� � �����
    [SerializeField] private Transform keyInsertPoint; // �����, ���� ����������� ����
    [SerializeField] private float rotationSpeed = 2f;  // �������� ��������
    [SerializeField] private float insertSpeed = 2f;    // �������� �������� �������/����������
    [SerializeField] private Vector3 insertOffset = new Vector3(0, 0, 0.1f); // �������� ��� �������� �������

    public IgnitionState currentIgnitionState = IgnitionState.Removed; // ��������� ���������
    private Quaternion targetRotation;         // ������� ������� �����
    private bool isAnimating = false;          // ���� �������� (�������, �������, ����������)
    private Vector3 insertedPosition;          // ������� ����� ����� �������
    private Vector3 removedPosition;           // ������� ����� ����� ��������/����� ����������

    public enum IgnitionState
    {
        Removed,   // 0 - ���� �����
        Inserted,  // 1 - ���� �������� (0 ��������)
        PowerMode, // 2 - ��������������� ������� (-45 ��������)
        Start      // 3 - ������ ��������� (-90 ��������)
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
            // ������������� ��������� ���������
            ignitionKey.SetActive(false); // ���� ���������� �������
            ignitionKey.transform.SetParent(keyInsertPoint); // ����������� ���� � ����� �������
            insertedPosition = keyInsertPoint.position; // ������� ������������ �����
            removedPosition = insertedPosition + insertOffset; // ������� ����� ��������/����� ����������
            ignitionKey.transform.position = removedPosition; // ��������� �������
            ignitionKey.transform.localRotation = Quaternion.Euler(0, 0, 0); // ��������� �������
        }
    }

    public void Interact()
    {
        if (isAnimating || ignitionKey == null) return;

        switch (currentIgnitionState)
        {
            case IgnitionState.Removed:
                currentIgnitionState = IgnitionState.Inserted;
                targetRotation = Quaternion.Euler(0, 0, 0); // ��������� ������� (0 ��������)
                StartCoroutine(InsertKey());
                break;

            case IgnitionState.Inserted:
                currentIgnitionState = IgnitionState.PowerMode;
                targetRotation = Quaternion.Euler(0, 45, 0); // ������� ������ ������� �� -45 ��������
                StartCoroutine(RotateKey());
                Off();
                PowerMode();
                break;

            case IgnitionState.PowerMode:
                currentIgnitionState = IgnitionState.Start;
                targetRotation = Quaternion.Euler(0, 90, 0); // ������� ������ ������� �� -90 ��������
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
            insertedPosition = keyInsertPoint.position; // ������� ������������ �����
            removedPosition = insertedPosition + insertOffset; // ������� ����� ��������/����� ����������
            ignitionKey.transform.position = removedPosition; // ��������� �������

        }
    }
    private IEnumerator InsertKey()
    {
        isAnimating = true;
        ignitionKey.SetActive(true); // ���������� ����

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

        ignitionKey.transform.localRotation = targetRotation; // ����������, ��� ������� ������
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
        ignitionKey.SetActive(false); // ������� ����
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