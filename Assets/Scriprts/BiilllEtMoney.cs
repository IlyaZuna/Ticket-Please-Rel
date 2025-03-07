using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiilllEtMoney : MonoBehaviour
{
    public int billValue; // Номинал купюры
    [SerializeField] private Vector3 newScale;
    public GameObject visualPrefab; // Префаб купюры для отображения рядом
    [SerializeField] private MoneySpawner moneySpawner; // Ссылка на объект-спавнер

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Увеличиваем сдачу
            DriverIncome.Instance.AddIncome(billValue);

            // Спавним визуальную копию купюры
            if (visualPrefab != null)
            {
                moneySpawner.SpawnMoney(visualPrefab, billValue, newScale);


            }
        }
    }
}
