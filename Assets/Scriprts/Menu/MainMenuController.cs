using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public SettingsManager settingsManager; // Ссылка на SettingsManager

    // Кнопка "Новая игра"
    public void OnNewGameButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Кнопка "Продолжить"
    public void OnContinueButton()
    {
        Debug.Log("Продолжить игру (добавь логику сохранения)");
    }

    // Кнопка "Настройки"
    public void OnSettingsButton()
    {
        settingsManager.OpenSettings();
    }

    // Кнопка "Выход"
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("Выход из игры");
    }
}