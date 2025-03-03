using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public SettingsManager settingsManager; // ������ �� SettingsManager

    // ������ "����� ����"
    public void OnNewGameButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // ������ "����������"
    public void OnContinueButton()
    {
        Debug.Log("���������� ���� (������ ������ ����������)");
    }

    // ������ "���������"
    public void OnSettingsButton()
    {
        settingsManager.OpenSettings();
    }

    // ������ "�����"
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("����� �� ����");
    }
}