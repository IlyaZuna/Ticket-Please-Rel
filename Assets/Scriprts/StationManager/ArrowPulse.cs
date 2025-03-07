using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPulse : MonoBehaviour
{
    private Material material;
    public float pulseSpeed = 1f; // Скорость пульсации
    public float minAlpha = 0.3f; // Минимальная прозрачность
    public float maxAlpha = 1f; // Максимальная прозрачность

    void Start()
    {
        material = GetComponent<Renderer>().material;
        // Убедимся, что материал имеет начальный цвет
        if (material.HasProperty("_MainTex"))
        {
            material.SetColor("_Color", Color.green); // Устанавливаем начальный цвет, если поддерживается
        }
    }

    void Update()
    {
        // Проверяем, поддерживает ли материал свойство _Color
        if (material.HasProperty("_Color"))
        {
            float alpha = Mathf.PingPong(Time.time * pulseSpeed, maxAlpha - minAlpha) + minAlpha;
            Color color = material.GetColor("_Color");
            color.a = alpha; // Меняем только альфа-канал
            material.SetColor("_Color", color);
        }
        else
        {
            Debug.LogWarning("Material does not support _Color property. Using fallback.");
            // Фолбек: работаем с альфа-каналом текстуры, если она есть
            if (material.HasProperty("_MainTex"))
            {
                float alpha = Mathf.PingPong(Time.time * pulseSpeed, maxAlpha - minAlpha) + minAlpha;
                material.SetTextureOffset("_MainTex", new Vector2(0, alpha)); // Упрощённый фолбек
            }
        }
    }
}