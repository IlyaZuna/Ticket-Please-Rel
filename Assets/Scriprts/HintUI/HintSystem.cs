using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintSystem : MonoBehaviour
{
    public TMP_Text hintText; // UI элемент для отображения подсказки
    private GameObject currentTarget; // Текущий объект, на который наведён луч

    void Start()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false); // Скрываем подсказку изначально
        }
    }

    public void ShowHint(GameObject target)
    {
        if (hintText == null) return;

        // Если наведённый объект сменился или это новый объект
        if (currentTarget != target)
        {
            currentTarget = target;

            // Ищем компонент HintData на объекте
            if (target.TryGetComponent(out HintData hintData))
            {
                hintText.text = hintData.GetHintText();
                hintText.gameObject.SetActive(true);
            }
            else
            {
                HideHint();
            }
        }
    }

    public void HideHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
        currentTarget = null;
    }
}
