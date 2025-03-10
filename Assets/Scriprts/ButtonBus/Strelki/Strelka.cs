using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strelka : MonoBehaviour
{
    [SerializeField] private BusController busController;
    private float minRotation = -50f; 
    private float maxRotation = 230f;
    private float AngleConvenshion = 1.56f;
    private float currentRotation = 0f;
    void Start()
    {
        busController = FindObjectOfType<BusController>();
    }   
    void Update()
    {   
            currentRotation = (busController.GetSpeed() * AngleConvenshion)+minRotation;
            currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation); // Ограничиваем угол
            transform.localRotation = Quaternion.Euler(-33, 0, currentRotation);
        
    }
}
