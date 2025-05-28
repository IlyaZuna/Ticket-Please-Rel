using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayValues : MonoBehaviour
{
    [Header("������ �� �������")]
    [SerializeField] private DriverIncome scriptA;
    [SerializeField] private ManagerStats scriptB;
    
    [Header("UI ��������")]
    [SerializeField] private TMP_Text textPassengers; // ���������
    [SerializeField] private TMP_Text textIncome;     // �����
    [SerializeField] private TMP_Text textUKP;        // ������� ���
    [SerializeField] private GameObject statsPanel;   // ������ �� ������ � ������������

    [Header("���������")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool updateEveryFrame = true;

    private void Start()
    {
        // �������� ������
        if (scriptA == null) Debug.LogError("DriverIncome �� ��������!");
        if (scriptB == null) Debug.LogError("ManagerStats �� ��������!");
        if (textPassengers == null || textIncome == null || textUKP == null) Debug.LogError("��������� ���� �� ���������!");
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
        if (scriptB != null)
        {
            textPassengers.text = $"���������: {scriptB.getPasengerSkore()}";
        }
        if (scriptA != null)
        {
            textIncome.text = $"�����: {scriptA.GetIncam()} ���.";
        }
        if (scriptB != null)
        {
            textUKP.text = $"���: {scriptB.GetUKPLevel()}";
        }
    }
}