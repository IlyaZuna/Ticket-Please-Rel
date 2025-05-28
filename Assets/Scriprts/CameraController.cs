using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // ���������������� ����
    public Transform playerBody;           // ������ �� ������� (������)

    private float xRotation = 0f;          // ������ ������� �� ��� X (����� � ����)
    private float yRotation = 180f;        // ������������� ��������� ������� �� ��� Y �� 180 ��������
    [SerializeField] private Camera playerCamera; // ������ ������
    [SerializeField] private float interactionDistance = 5f; // ��������� ��������������

    // ���������� ��� ���������
    private Material highlightMaterial;    // �������� ��� ���������
    private Renderer lastHighlightedRenderer; // ��������� ������������ ��������
    private Material originalMaterial;     // ������������ �������� �������
    [SerializeField] private Color outlineColor = Color.yellow; // ���� ���������
    [SerializeField] private float outlineWidth = 0.03f; // ������ �������

    // ������ �����, ������� Raycast ������ ������������
    [SerializeField] private string[] raycastIgnoredTags = { "RaycastIgnore" };
    // ������ �����, ������� ����� ������������ ��� ��������������
    [SerializeField] private string[] ignoredTags = { "BusStop" };

    // ������ �� ������� ���������
    [SerializeField] private HintSystem hintSystem; // ������ �� HintSystem

    private MapController mapController; // ������ �� MapController

    void Start()
    {
        // ��������� ������ � ������ ������
        Cursor.lockState = CursorLockMode.Locked;

        // ������������� ��������� ������� ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // ������������� ��������� ��� ���������
        highlightMaterial = new Material(Shader.Find("Custom/HighlightShader"));
        highlightMaterial.SetColor("_OutlineColor", outlineColor);
        highlightMaterial.SetFloat("_OutlineWidth", outlineWidth);

        if (hintSystem == null)
        {
            Debug.LogError("HintSystem is not assigned in the Inspector for CameraController!");
        }

        // ������� MapController � �����
        mapController = FindObjectOfType<MapController>();
    }

    private void Update()
    {
        // ���������, ������� �� �����
        if (mapController != null && mapController.IsMapOpen()) return;

        RayCaster();
    }

    void LateUpdate()
    {
        // ���������, ������� �� �����
        if (mapController != null && mapController.IsMapOpen()) return;

        // �������� �������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ����������� ��� Y ��� ����������� �������� ������ ����� � ����
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 35f);  // ����������� ���� �������� ������ ����� � ����

        // ������� �� ��� Y (����� � ������)
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, 130f, 340f);  // ����������� ���� �������� ������ ����� � ������

        // ��������� �������� � ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void RayCaster()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward; // ����������� ����� �� ������

        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus"); // ���������� ���� "Bus"

        RaycastHit hit;

        // ������������ ���� ��� �������
        Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, Color.red, 1f);

        // ��������� ������� � ������������ ���������
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // ���������, ����� �� ��� � ������ � �����, ������� ������������ Raycast'��
            foreach (string tag in raycastIgnoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    ResetHighlight(); // ���������� ���������
                    if (hintSystem != null) hintSystem.HideHint();
                    return; // ������� �� ������
                }
            }

            // ���������, ����� �� ��� � ������ � ������������ ����� ��� ��������������
            foreach (string tag in ignoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    ResetHighlight();
                    if (hintSystem != null) hintSystem.HideHint();
                    return;
                }
            }

            Renderer currentRenderer = hit.collider.GetComponent<Renderer>();

            // ������ ���������
            if (currentRenderer != null)
            {
                if (currentRenderer != lastHighlightedRenderer)
                {
                    ResetHighlight(); // ���������� ���������� ���������
                    originalMaterial = currentRenderer.material; // ��������� ������������ ��������
                    currentRenderer.material = highlightMaterial; // ��������� ���������
                    lastHighlightedRenderer = currentRenderer;
                }
            }

            // ���������� ���������, ���� ������ ����� ��������� HintData
            if (hintSystem != null)
            {
                hintSystem.ShowHint(hit.collider.gameObject);
            }

            Debug.Log($"��� ����� � ������: {hit.collider.name}");

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (Input.GetMouseButtonDown(0) && interactable != null)
            {
                interactable.Interact();
            }

            // ���������, ���� �� ��������� BillEtMoney
            var bill = hit.collider.GetComponent<BiilllEtMoney>();
            if (bill != null)
            {
                Debug.Log("������ � BillEtMoney ������!");
                bill.OnMouseDown();
            }
        }
        else
        {
            // ���� ��� �� �� ��� �� �����, ���������� ��������� � �������� ���������
            ResetHighlight();
            if (hintSystem != null) hintSystem.HideHint();
        }
    }

    private void ResetHighlight()
    {
        if (lastHighlightedRenderer != null)
        {
            lastHighlightedRenderer.material = originalMaterial;
            lastHighlightedRenderer = null;
        }
    }

    void OnDestroy()
    {
        // ������� ��������� �������� ��� ����������� �������
        if (highlightMaterial != null)
        {
            Destroy(highlightMaterial);
        }
    }
}