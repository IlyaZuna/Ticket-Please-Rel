using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class DisplayValues : MonoBehaviour
{
    [Header("������ �� �������")]
    [SerializeField] private ManagerStats scriptB;
    [SerializeField] private DriverIncome scriptA;

    [Header("UI ��������")]
    [SerializeField] private TMP_Text textA;
    [SerializeField] private TMP_Text textB;
    [SerializeField] private GameObject statsPanel; // ������ �� ������ � ������������

    [Header("���������")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool updateEveryFrame = true;

    private void Start()
    {
        // �������� ������
        if (scriptA == null) Debug.LogError("ScriptA (DriverIncome) �� ��������!");
        if (scriptB == null) Debug.LogError("ScriptB (ManagerStats) �� ��������!");
        if (textA == null || textB == null) Debug.LogError("��������� ���� �� ���������!");
        if (statsPanel == null) Debug.LogError("������ ���������� �� ���������!");

        // �������� ������ ��� ������
        statsPanel.SetActive(false);
    }

    private void Update()
    {
        // ������������ ������ �� ������� Tab
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }

        // ���������� ��������
        if (updateEveryFrame && statsPanel.activeSelf)
        {
            UpdateValues();
        }
    }

    private void TogglePanel()
    {
        bool newState = !statsPanel.activeSelf;
        statsPanel.SetActive(newState);

        // ��������� �������� ��� ��������
        if (newState)
        {
            UpdateValues();
        }
    }

    private void UpdateValues()
    {
        if (scriptB != null) textA.text = scriptB.getPasengerSkore().ToString();
        if (scriptA != null) textB.text = scriptA.GetIncam().ToString();
    }
}