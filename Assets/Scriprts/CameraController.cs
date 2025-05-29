using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;
    private float yRotation = 180f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 5f;

    // Переменные для Outline
    private Outline lastOutline; // Последний подсвеченный Outline
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 2f;
    [SerializeField] private Outline.Mode outlineMode = Outline.Mode.OutlineAll;

    [SerializeField] private string[] raycastIgnoredTags = { "RaycastIgnore" };
    [SerializeField] private string[] ignoredTags = { "BusStop" };

    [SerializeField] private HintSystem hintSystem;
    private MapController mapController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (hintSystem == null)
        {
            Debug.LogError("HintSystem is not assigned in the Inspector for CameraController!");
        }

        mapController = FindObjectOfType<MapController>();
    }

    private void Update()
    {
        if (mapController != null && mapController.IsMapOpen()) return;
        RayCaster();
    }

    void LateUpdate()
    {
        if (mapController != null && mapController.IsMapOpen()) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 35f);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, 130f, 340f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void RayCaster()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus");

        RaycastHit hit;
        Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // Проверка игнорируемых тегов
            foreach (string tag in raycastIgnoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    ResetOutline();
                    if (hintSystem != null) hintSystem.HideHint();
                    return;
                }
            }

            foreach (string tag in ignoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    ResetOutline();
                    if (hintSystem != null) hintSystem.HideHint();
                    return;
                }
            }

            // Получаем или добавляем компонент Outline
            Outline currentOutline = hit.collider.GetComponent<Outline>();
            if (currentOutline == null)
            {
                currentOutline = hit.collider.gameObject.AddComponent<Outline>();
                currentOutline.OutlineMode = outlineMode;
                currentOutline.OutlineColor = outlineColor;
                currentOutline.OutlineWidth = outlineWidth;
            }
            else
            {
                currentOutline.enabled = true;
            }

            if (currentOutline != lastOutline)
            {
                ResetOutline();
                lastOutline = currentOutline;
            }

            // Показ подсказки
            if (hintSystem != null)
            {
                hintSystem.ShowHint(hit.collider.gameObject);
            }

            Debug.Log($"Рэй попал в объект: {hit.collider.name}");

            // Взаимодействие
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (Input.GetMouseButtonDown(0) && interactable != null)
            {
                interactable.Interact();
            }

            var bill = hit.collider.GetComponent<BiilllEtMoney>();
            if (bill != null)
            {
                Debug.Log("Попали в BillEtMoney объект!");
                bill.OnMouseDown();
            }
        }
        else
        {
            ResetOutline();
            if (hintSystem != null) hintSystem.HideHint();
        }
    }

    private void ResetOutline()
    {
        if (lastOutline != null)
        {
            lastOutline.enabled = false;
            lastOutline = null;
        }
    }
}