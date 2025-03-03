using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ��� ������ � TextMeshPro
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanelPrefab; // ������ ������ ��������
    private GameObject settingsPanelInstance; // ��������� ������ ��������
    private Slider volumeSlider; // ������� ���������
    private TMP_Dropdown qualityDropdown; // Dropdown ��� �������� �������
    private TMP_Dropdown resolutionDropdown; // Dropdown ��� ����������
    private GameObject backToMenuButton; // ������ "����� � ����"
    private GameObject exitGameButton; // ������ "����� �� ����"
    private bool isPaused = false; // ��� ������������ �����
    private bool wasCursorVisible; // ��� ���������� ��������� �������
    private CursorLockMode wasCursorLockState; // ��� ���������� ��������� ���������� �������

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // ��������� MenuManager ����� �������
    }

    void Start()
    {
        // ��������� ������ ������ ��������
        if (settingsPanelPrefab != null)
        {
            SetupSettingsPanel();
        }
        else
        {
            Debug.LogError("SettingsPanelPrefab �� �������� � Inspector!");
        }
    }

    void Update()
    {
        // ��������/�������� �������� ����� Esc � �������� �����
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

    // ��������� ������ ��������
    private void SetupSettingsPanel()
    {
        // ������� Canvas � ������� �����
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Canvas �� ������ � �����! ������ ����� Canvas...");
            GameObject canvasObject = new GameObject("DynamicCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<CanvasRenderer>();
        }

        // ������ ��������� ������ ��������
        settingsPanelInstance = Instantiate(settingsPanelPrefab);
        settingsPanelInstance.transform.SetParent(canvas.transform, false);
        Debug.Log("SettingsPanel ������ � �������� � Canvas: " + canvas.name);

        // ������������� RectTransform ��� ��������
        RectTransform rect = settingsPanelInstance.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        // ������� UI-�������� ������ ������ ��������
        volumeSlider = settingsPanelInstance.GetComponentInChildren<Slider>();
        TMP_Dropdown[] dropdowns = settingsPanelInstance.GetComponentsInChildren<TMP_Dropdown>();
        qualityDropdown = dropdowns.Length > 0 ? dropdowns[0] : null; // ������ Dropdown (��������)
        resolutionDropdown = dropdowns.Length > 1 ? dropdowns[1] : null; // ������ Dropdown (����������)

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

        // ��������� ���������� ���������
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        else
        {
            Debug.LogWarning("VolumeSlider �� ������ � SettingsPanel!");
        }

        if (qualityDropdown != null)
        {
            int savedQuality = PlayerPrefs.GetInt("Quality", 1); // �� ��������� "Medium" (������ 1)
            qualityDropdown.value = savedQuality;
            ApplyQualitySetting(savedQuality);
            qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        }
        else
        {
            Debug.LogWarning("QualityDropdown �� ������ � SettingsPanel!");
        }

        if (resolutionDropdown != null)
        {
            int savedResolution = PlayerPrefs.GetInt("Resolution", 2); // �� ��������� 1920x1080 (������ 2)
            resolutionDropdown.value = savedResolution;
            ApplyResolutionSetting(savedResolution);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }
        else
        {
            Debug.LogWarning("ResolutionDropdown �� ������ � SettingsPanel!");
        }

        // ��������� ������ �������� ��� ������
        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(false);
        }
    }

    // �������� ��������
    public void OpenSettings()
    {
        // ���� ��������� ������ �� ����������, ������ ��� ������
        if (settingsPanelInstance == null)
        {
            SetupSettingsPanel();
        }

        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(true);
            // ����������/�������� ������ � ����������� �� �����
            bool isMainScene = SceneManager.GetActiveScene().name != "MainMenu";
            if (backToMenuButton != null) backToMenuButton.SetActive(isMainScene);
            if (exitGameButton != null) exitGameButton.SetActive(isMainScene);

            // ��������� ������� ��������� �������
            wasCursorVisible = Cursor.visible;
            wasCursorLockState = Cursor.lockState;

            // ������ ������ ������� � ����������������
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("������ ��� �������� ��������: visible=" + Cursor.visible + ", lockState=" + Cursor.lockState);

            // ������ ���� �� �����, ���� �� � ������� ����
            if (isMainScene)
            {
                Time.timeScale = 0f;
                isPaused = true;
            }
        }
        else
        {
            Debug.LogError("�� ������� ������� ���������: settingsPanelInstance ����� null!");
        }
    }

    // �������� ��������
    public void CloseSettings()
    {
        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(false);
            // ���������� ������ � �������� ���������
            Cursor.visible = wasCursorVisible;
            Cursor.lockState = wasCursorLockState;
            Debug.Log("������ ��� �������� ��������: visible=" + Cursor.visible + ", lockState=" + Cursor.lockState);

            // ������� �����, ���� ���� � �������� �����
            if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
            }
        }
    }

    // ��������� ���������
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    // ��������� �������� �������
    public void OnQualityChanged(int index)
    {
        ApplyQualitySetting(index);
        PlayerPrefs.SetInt("Quality", index);
    }

    // ���������� ��������� ��������
    private void ApplyQualitySetting(int index)
    {
        switch (index)
        {
            case 0: // ������
                QualitySettings.SetQualityLevel(0, true);
                break;
            case 1: // �������
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 2: // �������
                QualitySettings.SetQualityLevel(4, true);
                break;
        }
    }

    // ��������� ����������
    public void OnResolutionChanged(int index)
    {
        ApplyResolutionSetting(index);
        PlayerPrefs.SetInt("Resolution", index);
    }

    // ���������� ����������
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

    // �������� �������� (��� ������)
    public void OnCloseSettingsButton()
    {
        CloseSettings();
    }

    // ����� � ������� ����
    public void OnBackToMenuButton()
    {
        Time.timeScale = 1f; // ������� �����
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    // ������ ����� �� ����
    public void OnExitGameButton()
    {
        Application.Quit();
        Debug.Log("����� �� ����");
    }
}