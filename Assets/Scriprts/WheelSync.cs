using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSync : MonoBehaviour
{
    public WheelCollider wheelCollider;
    public Transform wheelModel;

    void Update()
    {
        // Синхронизация позиции
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelModel.position = pos;
        wheelModel.rotation = rot;
    }
}