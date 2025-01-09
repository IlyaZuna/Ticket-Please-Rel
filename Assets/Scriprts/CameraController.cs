using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // ���������������� ����
    public Transform playerBody;           // ������ �� ������� (������)

    private float xRotation = 0f;          // ������ ������� �� ��� X (����� � ����)
    private float yRotation = 180f;        // ������������� ��������� ������� �� ��� Y �� 180 ��������
    [SerializeField] private Camera playerCamera; // ������ ������
    [SerializeField] private float interactionDistance = 5f; // ��������� ��������������

    void Start()
    {
        // ��������� ������ � ������ ������
        Cursor.lockState = CursorLockMode.Locked;

        // ������������� ��������� ������� ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Update()
    {
        // �������� �������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ����������� ��� Y ��� ����������� �������� ������ ����� � ����
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);  // ����������� ���� �������� ������ ����� � ����

        // ������� �� ��� Y (����� � ������)
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, 120f, 260f);  // ����������� ���� �������� ������ ����� � ������

        // ��������� �������� � ������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        RayCaster();
    }
    private void RayCaster()
    {
        Vector3 rayOrigin = playerCamera.transform.position;

        // ����������� ����. ��� ����� ���� ����� ����������� �� ������, ��������, ������ �� ��� Z
        Vector3 rayDirection = playerCamera.transform.forward;  // ����������� ������ �� ������

        // ������� ��� �� ������
        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus");

        RaycastHit hit;

        // ��������� ������� � ������������ ���������
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // ���������, ����� �� ��� � ������
            Debug.Log($"��� ����� � ������: {hit.collider.name}");

            // ���������, ���� �� ��������� BillEtMoney �� �������, � ������� ����� ���
            var bill = hit.collider.GetComponent<BiilllEtMoney>();
            if (bill != null)
            {
                // ���� ��������� ������, ������ ���-�� � ���� ��������
                Debug.Log("������ � BillEtMoney ������!");
                // ����� ����� ������� �����, ��������, OnMouseDown()
                bill.OnMouseDown(); // ����� ������ �� BillEtMoney ����������
            }
        }



    }
}
