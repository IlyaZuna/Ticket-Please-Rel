using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private Transform parentObject; // ������������ ������ ��� ���� �����
    [SerializeField] private float stackOffset = 0.1f; // ���������� ����� �������� � ������
    [SerializeField] private Transform spawnPoz;
    private int stackCount = 0; // ������� ��� ������������ ������ ������
    private List<GameObject> spawnedMoney = new List<GameObject>(); // ������ ��� �������� ������������ ��������
    public void SpawnMoney(GameObject prefab, int value)
    {
         
        // ��������� ������� ��� ������ ������� � ������ ������
        Vector3 spawnPosition = spawnPoz.position + new Vector3(0, stackCount * stackOffset, 0);

        Quaternion rotation = Quaternion.Euler(-90, 175, Random.Range(-150, -200));
        // ������� ������
        GameObject newMoney = Instantiate(prefab, spawnPosition, rotation);

        // ������������� ��������
        if (parentObject != null)
        {
            newMoney.transform.SetParent(parentObject);
        }

        // ����������� �������
        newMoney.transform.localScale = new Vector3(20, 10, 20);
        spawnedMoney.Add(newMoney);
        // ����������� ������� ������
        stackCount++;

        // (�����������) �������� ���������, ����� ������ ���� ���� ��������
        var billComponent = newMoney.GetComponent<BiilllEtMoney>();
        if (billComponent != null)
        {
            billComponent.billValue = value; // �������� �������
        }
    }

    public void ResetStack()
    {
        foreach (GameObject money in spawnedMoney)
        {
            if (money != null)
            {
                Destroy(money);
            }
        }

        // ������� ������ � ���������� �������
        spawnedMoney.Clear();
        stackCount = 0;
        
    }
}
