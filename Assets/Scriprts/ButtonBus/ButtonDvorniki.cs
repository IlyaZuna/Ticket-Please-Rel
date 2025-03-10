using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDvorniki : MonoBehaviour
{
    [SerializeField] private Dworn dworn;
    public void Interact()
    {
        dworn.ONN();
    }
}

