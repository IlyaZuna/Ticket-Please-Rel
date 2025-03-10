using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dworn : MonoBehaviour
{
    public float speed = 100f; // �������� ��������
    public float minAngle = -30f; // ����������� ���� �������
    public float maxAngle = 30f; // ������������ ���� �������
    [SerializeField]private float currentAngle;
    private bool movingForward = true;

    void Update()
    {
        RotateWipers();
    }

    void RotateWipers()
    {
        // ��������� ���� ��������
        if (movingForward)
        {
            currentAngle += speed * Time.deltaTime;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                movingForward = false;
            }
        }
        else
        {
            currentAngle -= speed * Time.deltaTime;
            if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;
                movingForward = true;
            }
        }

        // ��������� �������
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, currentAngle, transform.localRotation.eulerAngles.z);
    }
}