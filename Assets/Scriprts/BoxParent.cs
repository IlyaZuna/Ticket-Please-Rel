using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxParent : MonoBehaviour
{
    [SerializeField] private string[] allowedTags; // Массив разрешённых тегов
   
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли тег в списке разрешённых
        if (IsTagAllowed(other.tag))
        {
            other.transform.SetParent(transform); // Делаем объект дочерним
            Debug.Log($"{other.gameObject.name} присоединён к платформе.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, есть ли тег в списке разрешённых
        if (IsTagAllowed(other.tag))
        {
            other.transform.SetParent(null); // Убираем объект из родителя
            Debug.Log($"{other.gameObject.name} покинул платформу.");
        }
    }

    // Проверяем, есть ли тег в массиве
    private bool IsTagAllowed(string tag)
    {
        foreach (var allowedTag in allowedTags)
        {
            if (tag == allowedTag)
            {
                return true;
            }
        }
        return false;
    }
}