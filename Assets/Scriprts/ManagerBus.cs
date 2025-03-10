using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBus : MonoBehaviour
{
    public BusController busController; // Скрипт управления автобусом  
    [Header("1 шаг запуска питание")]
    public MonoBehaviour[] Power; // Массив всех скриптов, которые нужно включать/выключать        
    [Header("2 шаг запуска двигатель")]
    public MonoBehaviour[] Engine;
    void Start()
    {
        StepOne(false); // По умолчанию  выключен
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