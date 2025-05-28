using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private BusController controller;
    [SerializeField] private BusDoorController busDoorController1;
    [SerializeField] private BusDoorController busDoorController2;
    [SerializeField] private BusDoorController busDoorController3;
    [SerializeField] private BusDoorController busDoorController4;
    public static event Action OnButtonPressed;
    public void Interact()
    {
        busDoorController1.ToggleDoor();
        busDoorController2.ToggleDoor();
        busDoorController3.ToggleDoor();
        busDoorController4.ToggleDoor();
        controller.areDoorsOpen = true;
    }
}
