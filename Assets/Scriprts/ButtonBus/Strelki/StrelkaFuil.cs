using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrelkaFuil : MonoBehaviour
{
    [SerializeField] private BusController busController;
    private float minRotation = 50f;
    private float maxRotation = 125f;
    private float AngleConvenshion =0.75f;
    private float currentRotation = 50f;
    void Start()
    {
        busController = FindObjectOfType<BusController>();
    }
    void Update()
    {
        currentRotation = (busController.GetFuil() * AngleConvenshion) + minRotation;
        currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation); // Ограничиваем угол
        transform.localRotation = Quaternion.Euler(-33, 0, currentRotation);

    }
}
