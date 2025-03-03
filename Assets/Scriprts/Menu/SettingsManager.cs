using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Для работы с TextMeshPro
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanelPrefab; // Префаб панели настроек
    private GameObject settingsPanelInstance; // Экземпляр панели настроек
    private Slider volumeSlider; // Слайдер громкости
    private TMP_Dropdown qualityDropdown; // Dropdown для качества графики
    private TMP_Dropdown resolutionDropdown; // Dropdown для разрешения
    private GameObject backToMenuButton; // Кнопка "Выйти в меню"
    private GameObject exitGameButton; // Кнопка "Выйти из игры"
    private bool isPaused = false; // Для отслеживания паузы
    private bool wasCursorVisible; // Для сохранения состояния курсора
    private CursorLockMode wasCursorLockState; // Для сохранения состояния блокировки курсора

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Сохраняем MenuManager между сценами
    }

    void Start()
    {
        // Загружаем префаб панели настроек
        if (settingsPanelPrefab != null)
        {
            SetupSettingsPanel();
        }
        else
        {
            Debug.LogError("SettingsPanelPrefab не привязан в Inspector!");
        }
    }

    void Update()
    {
        // Открытие/закрытие настроек через Esc в основной сцене
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanelInstance != null && settingsPanelInstance.activeSelf)
            {
                CloseSettings();
            }
            else if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                OpenSettings();
            }
        }
    }

    // Настройка панели настроек
    private void SetupSettingsPanel()
    {
        // Находим Canvas в текущей сцене
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Canvas не найден в сцене! Создаём новый Canvas...");
            GameObject canvasObject = new GameObject("DynamicCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<CanvasRenderer>();
        }

        // Создаём экземпляр панели настроек
        settingsPanelInstance = Instantiate(settingsPanelPrefab);
        settingsPanelInstance.transform.SetParent(canvas.transform, false);
        Debug.Log("SettingsPanel создан и привязан к Canvas: " + canvas.name);

        // Устанавливаем RectTransform для растяжки
        RectTransform rect = settingsPanelInstance.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        // Находим UI-элементы внутри панели настроек
        volumeSlider = settingsPanelInstance.GetComponentInChildren<Slider>();
        TMP_Dropdown[] dropdowns = settingsPanelInstance.GetComponentsInChildren<TMP_Dropdown>();
        qualityDropdown = dropdowns.Length > 0 ? dropdowns[0] : null; // Первый Dropdown (качество)
        resolutionDropdown = dropdowns.Length > 1 ? dropdowns[1] : null; // Второй Dropdown (разрешение)

        Button[] buttons = settingsPanelInstance.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.name == "CloseSettingsButton")
            {
                button.onClick.AddListener(OnCloseSettingsButton);
            }
            else if (button.name == "BackToMenuButton")
            {
                backToMenuButton = button.gameObject;
                button.onClick.AddListener(OnBackToMenuButton);
            }
            else if (button.name == "ExitGameButton")
            {
                exitGameButton = button.gameObject;
                button.onClick.AddListener(OnExitGameButton);
            }
        }

        // Загружаем сохранённые настройки
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        else
        {
            Debug.LogWarning("VolumeSlider не найден в SettingsPanel!");
        }

        if (qualityDropdown != null)
        {
            int savedQuality = PlayerPrefs.GetInt("Quality", 1); // По умолчанию "Medium" (индекс 1)
            qualityDropdown.value = savedQuality;
            ApplyQualitySetting(savedQuality);
            qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        }
        else
        {
            Debug.LogWarning("QualityDropdown не найден в SettingsPanel!");
        }

        if (resolutionDropdown != null)
        {
            int savedResolution = PlayerPrefs.GetInt("Resolution", 2); // По умолчанию 1920x1080 (индекс 2)
            resolutionDropdown.value = savedResolution;
            ApplyResolutionSetting(savedResolution);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }
        else
        {
            Debug.LogWarning("ResolutionDropdown не найден в SettingsPanel!");
        }

        // Отключаем панель настроек при старте
        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(false);
        }
    }

    // Открытие настроек
    public void OpenSettings()
    {
        // Если экземпляр панели не существует, создаём его заново
        if (settingsPanelInstance == null)
        {
            SetupSettingsPanel();
        }

        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(true);
            // Показываем/скрываем кнопки в зависимости от сцены
            bool isMainScene = SceneManager.GetActiveScene().name != "MainMenu";
            if (backToMenuButton != null) backToMenuButton.SetActive(isMainScene);
            if (exitGameButton != null) exitGameButton.SetActive(isMainScene);

            // Сохраняем текущее состояние курсора
            wasCursorVisible = Cursor.visible;
            wasCursorLockState = Cursor.lockState;

            // Делаем курсор видимым и разблокированным
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Курсор при открытии настроек: visible=" + Cursor.visible + ", lockState=" + Cursor.lockState);

            // Ставим игру на паузу, если не в главном меню
            if (isMainScene)
            {
                Time.timeScale = 0f;
                isPaused = true;
            }
        }
        else
        {
            Debug.LogError("Не удалось открыть настройки: settingsPanelInstance равен null!");
        }
    }

    // Закрытие настроек
    public void CloseSettings()
    {
        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(false);
            // Возвращаем курсор в исходное состояние
            Cursor.visible = wasCursorVisible;
            Cursor.lockState = wasCursorLockState;
            Debug.Log("Курсор при закрытии настроек: visible=" + Cursor.visible + ", lockState=" + Cursor.lockState);

            // Снимаем паузу, если были в основной сцене
            if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
            }
        }
    }

    // Изменение громкости
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    // Изменение качества графики
    public void OnQualityChanged(int index)
    {
        ApplyQualitySetting(index);
        PlayerPrefs.SetInt("Quality", index);
    }

    // Применение настройки качества
    private void ApplyQualitySetting(int index)
    {
        switch (index)
        {
            case 0: // Низкое
                QualitySettings.SetQualityLevel(0, true);
                break;
            case 1: // Среднее
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 2: // Высокое
                QualitySettings.SetQualityLevel(4, true);
                break;
        }
    }

    // Изменение разрешения
    public void OnResolutionChanged(int index)
    {
        ApplyResolutionSetting(index);
        PlayerPrefs.SetInt("Resolution", index);
    }

    // Применение разрешения
    private void ApplyResolutionSetting(int index)
    {
        switch (index)
        {
            case 0: // 800x600
                Screen.SetResolution(800, 600, Screen.fullScreen);
                break;
            case 1: // 1280x720
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 2: // 1920x1080
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 3: // 2560x1440
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
        }
    }

    // Закрытие настроек (для кнопки)
    public void OnCloseSettingsButton()
    {
        CloseSettings();
    }

    // Выход в главное меню
    public void OnBackToMenuButton()
    {
        Time.timeScale = 1f; // Снимаем паузу
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    // Полный выход из игры
    public void OnExitGameButton()
    {
        Application.Quit();
        Debug.Log("Выход из игры");
    }
}