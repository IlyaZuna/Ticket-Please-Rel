using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private Transform parentObject; // Родительский объект для всех денег
    [SerializeField] private float stackOffset = 0.1f; // Расстояние между деньгами в стопке
    [SerializeField] private Transform spawnPoz;
    private int stackCount = 0; // Счетчик для отслеживания высоты стопки
    private List<GameObject> spawnedMoney = new List<GameObject>(); // Список для хранения заспавненных объектов
    public void SpawnMoney(GameObject prefab, int value)
    {
         
        // Вычисляем позицию для нового объекта с учетом стопки
        Vector3 spawnPosition = spawnPoz.position + new Vector3(0, stackCount * stackOffset, 0);

        Quaternion rotation = Quaternion.Euler(-90, 175, Random.Range(-150, -200));
        // Спавним объект
        GameObject newMoney = Instantiate(prefab, spawnPosition, rotation);

        // Устанавливаем родителя
        if (parentObject != null)
        {
            newMoney.transform.SetParent(parentObject);
        }

        // Настраиваем масштаб
        newMoney.transform.localScale = new Vector3(20, 10, 20);
        spawnedMoney.Add(newMoney);
        // Увеличиваем счетчик стопки
        stackCount++;

        // (Опционально) Добавить компонент, чтобы объект знал свою ценность
        var billComponent = newMoney.GetComponent<BiilllEtMoney>();
        if (billComponent != null)
        {
            billComponent.billValue = value; // Передаем номинал
        }
    }

    public void ResetStack()
    {
        foreach (GameObject money in spawnedMoney)
        {
            if (money != null)
            {
                Destroy(money);
            }
        }

        // Очищаем список и сбрасываем счетчик
        spawnedMoney.Clear();
        stackCount = 0;
        
    }
}
