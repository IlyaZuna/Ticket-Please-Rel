using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBusLights : MonoBehaviour, IInteractable
{
    [Tooltip("���������� ���� ������������ ������ �� ����� ������� ������")]
    [SerializeField] private GameObject lightsParent; // �������� ���� ����

    [Tooltip("��������� ��������� ����� (�������� �� ���������)")]
    [SerializeField] private bool startDisabled = true;

    private void Start()
    {
        // ��������� ���� ��� ������, ���� �����
        if (startDisabled && lightsParent != null)
        {
            lightsParent.SetActive(false);
        }
    }

    public void Interact()
    {
        if (lightsParent != null)
        {
            // ����������� ��������� (���/����)
            lightsParent.SetActive(!lightsParent.activeSelf);

            // ����� �������� ���� ������� ��� ������ �������
            Debug.Log("���� ������: " + (lightsParent.activeSelf ? "���" : "����"));
        }
        else
        {
            Debug.LogError("�� �������� ������ � �������!", this);
        }
    }
}