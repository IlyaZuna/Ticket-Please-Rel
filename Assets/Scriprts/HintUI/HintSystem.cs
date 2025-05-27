using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintSystem : MonoBehaviour
{
    public TMP_Text hintText; // UI ������� ��� ����������� ���������
    private GameObject currentTarget; // ������� ������, �� ������� ������ ���

    void Start()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false); // �������� ��������� ����������
        }
    }

    public void ShowHint(GameObject target)
    {
        if (hintText == null) return;

        // ���� ��������� ������ �������� ��� ��� ����� ������
        if (currentTarget != target)
        {
            currentTarget = target;

            // ���� ��������� HintData �� �������
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
