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
    private bool ON = false;

    void Update()
    {
        if (ON)
        {
            RotateWipers();
        }
        else
        {
            GoStartPoz();
        }
    }
    public void ONN( )
    {
        ON = !ON;
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
    private void GoStartPoz()
    {
        if (currentAngle <= minAngle)
        {
            currentAngle = minAngle;          
            currentAngle -= speed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, currentAngle, transform.localRotation.eulerAngles.z);
        }
        else
        {
            return;
        }
    }
}