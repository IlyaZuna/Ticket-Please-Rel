using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiilllEtMoney : MonoBehaviour
{
    public int billValue; // ������� ������
    [SerializeField] private Vector3 newScale;
    public GameObject visualPrefab; // ������ ������ ��� ����������� �����
    [SerializeField] private MoneySpawner moneySpawner; // ������ �� ������-�������

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ����������� �����
            DriverIncome.Instance.AddIncome(billValue);

            // ������� ���������� ����� ������
            if (visualPrefab != null)
            {
                moneySpawner.SpawnMoney(visualPrefab, billValue, newScale);


            }
        }
    }
}
