using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwornicPalca : MonoBehaviour
{
    void Update()
    {
        // ��������� ������ �� (0, 0, 180) � ���������� �����������
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
