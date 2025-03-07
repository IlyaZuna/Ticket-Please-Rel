using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float floatAmplitude = 0.1f; // ��������� ��������� �� ������
    public float floatSpeed = 2f; // �������� ���������

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; // ��������� ��������� �������
    }

    void Update()
    {
        // ��������� �� ������
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}