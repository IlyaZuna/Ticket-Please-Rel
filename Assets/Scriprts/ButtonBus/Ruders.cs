using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruders : MonoBehaviour
{
    public float turnSpeed = 300f;   // �������� �������� ����
    public float returnSpeed = 5f;   // �������� �������� � �����
    private float maxRotation = 900f; // 2.5 ������� (360 * 2.5)
    private float currentRotation = 0f; // ������� ���� �������� ����

    void Update()
    {
        float turnInput = Input.GetAxis("Horizontal"); // �������� ���� �� ������

        if (Mathf.Abs(turnInput) > 0.01f)
        {
            // ���� ���� ����, ������� ���� �����/������
            currentRotation += turnInput * turnSpeed * Time.deltaTime;
            currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation); // ������������ ����
        }
        else
        {
            // ���� ������� �� ������, ������ ���������� ���� � 0�
            currentRotation = Mathf.Lerp(currentRotation, 0f, returnSpeed * Time.deltaTime);
        }

        // ��������� �������� ������ � ���� �� ��� Z
        transform.localRotation = Quaternion.Euler(-75, 0, currentRotation);
    }
}

