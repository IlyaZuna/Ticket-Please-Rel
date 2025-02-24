using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxParent : MonoBehaviour
{
    [SerializeField] private string[] allowedTags; // ������ ����������� �����
   
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ���� �� ��� � ������ �����������
        if (IsTagAllowed(other.tag))
        {
            other.transform.SetParent(transform); // ������ ������ ��������
            Debug.Log($"{other.gameObject.name} ���������� � ���������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���������, ���� �� ��� � ������ �����������
        if (IsTagAllowed(other.tag))
        {
            other.transform.SetParent(null); // ������� ������ �� ��������
            Debug.Log($"{other.gameObject.name} ������� ���������.");
        }
    }

    // ���������, ���� �� ��� � �������
    private bool IsTagAllowed(string tag)
    {
        foreach (var allowedTag in allowedTags)
        {
            if (tag == allowedTag)
            {
                return true;
            }
        }
        return false;
    }
}