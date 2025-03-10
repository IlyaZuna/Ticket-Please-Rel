using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrelkaTaxometr : MonoBehaviour
{
    [SerializeField] private BusController busController;
    private float minRotation = -24f;
    private float maxRotation = 230f;
    private float currentRotation = 0f;
    private float AngleConvenshion = 0.035f;
    public float minRPM = 800f;  // �������� ���
    public float maxRPM = 6000f; // ������������ �������
    void Start()
    {
        busController = FindObjectOfType<BusController>();
    }
    void Update()
    {
        
        currentRotation = Mathf.Lerp(minRPM, maxRPM, busController.GetSpeed() / 100f);
        currentRotation = (currentRotation * AngleConvenshion)+minRotation;
        currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation); // ������������ ����
        transform.localRotation = Quaternion.Euler(-33, 0, currentRotation);

    }
}
