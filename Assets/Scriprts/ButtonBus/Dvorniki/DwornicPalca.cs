using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwornicPalca : MonoBehaviour
{
    void Update()
    {
        // Фиксируем объект на (0, 0, 180) в глобальных координатах
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
