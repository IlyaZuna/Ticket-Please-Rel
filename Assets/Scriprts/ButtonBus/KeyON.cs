using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyON : MonoBehaviour, IInteractable
{
    [SerializeField] private BusController busController;
    [SerializeField] private ManagerBus manager;
    public IgnitionState currentIgnitionState = IgnitionState.Off;
    void Start()
    {
        busController = FindObjectOfType<BusController>();
    }
    public enum IgnitionState
    {
        Off,       // 0 - Выключено
        PowerMode, // 1 - Вспомогательное питание (радио, фары)       
        Start      // 3 - Запуск двигателя
    }
    public void Interact()
    {
        switch (currentIgnitionState)
        {
            case IgnitionState.Off:              
                currentIgnitionState = IgnitionState.PowerMode;
                Off();
                break;

            case IgnitionState.PowerMode:               
                currentIgnitionState = IgnitionState.Start;
                PowerMode();
                break;
            case IgnitionState.Start:               
                currentIgnitionState = IgnitionState.Off;
                StartEngine();
                break;
            
        }
    }
    void Off()
    {
        manager.StepOne(false);
        manager.StepTwo(false);
    }
    void PowerMode()
    {
        manager.StepOne(true);
    }  

    void StartEngine()
    {
        manager.StepTwo(true);
    }
    
    
}
