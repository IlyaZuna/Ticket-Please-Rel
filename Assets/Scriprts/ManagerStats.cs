using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStats : MonoBehaviour
{
    public int _pasengerSkore;      // ��������� �� ������� �����
    public int _pasengerallSkore;   // ��� ��������� �� �� �����
    public int day;                 // ����� ���
    private DriverIncome driverIncome; // ������ �� DriverIncome
    public int allIncame;           // ����� �����

    void Start()
    {
        driverIncome = FindObjectOfType<DriverIncome>(); // ������������� DriverIncome
        if (driverIncome == null)
        {
            Debug.LogError("DriverIncome not found in scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPasengerSkore()
    {
        _pasengerSkore++;
        return;
    }

    public void addPasengerallSkore()
    {
        _pasengerallSkore++;
        return;
    }

    public void addDay()
    {
        day++;
        addPasengerallSkore();
        allIncame = driverIncome != null ? driverIncome.Incame() : 0; // ���������� ��������
        return;
    }

    public int getPasengerSkore()
    {
        return _pasengerSkore;
    }

    // �������� ��� ������ ��� (��������, �� 1 �� 5)
    public int GetUKPLevel()
    {
        return 100; // ��������, ����� �������� �� �������� ������
    }
}