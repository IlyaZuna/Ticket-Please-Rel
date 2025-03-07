using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPulse : MonoBehaviour
{
    private Material material;
    public float pulseSpeed = 1f; // �������� ���������
    public float minAlpha = 0.3f; // ����������� ������������
    public float maxAlpha = 1f; // ������������ ������������

    void Start()
    {
        material = GetComponent<Renderer>().material;
        // ��������, ��� �������� ����� ��������� ����
        if (material.HasProperty("_MainTex"))
        {
            material.SetColor("_Color", Color.green); // ������������� ��������� ����, ���� ��������������
        }
    }

    void Update()
    {
        // ���������, ������������ �� �������� �������� _Color
        if (material.HasProperty("_Color"))
        {
            float alpha = Mathf.PingPong(Time.time * pulseSpeed, maxAlpha - minAlpha) + minAlpha;
            Color color = material.GetColor("_Color");
            color.a = alpha; // ������ ������ �����-�����
            material.SetColor("_Color", color);
        }
        else
        {
            Debug.LogWarning("Material does not support _Color property. Using fallback.");
            // ������: �������� � �����-������� ��������, ���� ��� ����
            if (material.HasProperty("_MainTex"))
            {
                float alpha = Mathf.PingPong(Time.time * pulseSpeed, maxAlpha - minAlpha) + minAlpha;
                material.SetTextureOffset("_MainTex", new Vector2(0, alpha)); // ���������� ������
            }
        }
    }
}