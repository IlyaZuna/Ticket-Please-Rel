using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBus : MonoBehaviour
{
    public BusController busController; // ������ ���������� ���������  
    [Header("1 ��� ������� �������")]
    public MonoBehaviour[] Power; // ������ ���� ��������, ������� ����� ��������/���������        
    [Header("2 ��� ������� ���������")]
    public MonoBehaviour[] Engine;
    void Start()
    {
        StepOne(false); // �� ���������  ��������
        StepTwo(false);
    }
   
    public void StepOne(bool state)
    {       
        foreach (var script in Power)
        {
            if (script != null)
            {
                script.enabled = state;
            }
        }      
    }
    public void StepTwo(bool state)
    {      

        foreach (var script in Engine)
        {
            if (script != null)
            {
                script.enabled = state;
            }
        }
        busController.Engine(state);
        
    }
}