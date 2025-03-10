using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDworniki : MonoBehaviour, IInteractable
{

    public static event Action OnButtonPressed;
    public void Interact()
    {
        OnButtonPressed?.Invoke();
    }
}
