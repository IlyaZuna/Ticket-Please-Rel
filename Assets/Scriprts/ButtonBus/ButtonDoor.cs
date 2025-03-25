using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonDoor : MonoBehaviour, IInteractable
{
    public static event Action OnButtonPressed;
    public void Interact()
    {
        OnButtonPressed?.Invoke();
    }
}
