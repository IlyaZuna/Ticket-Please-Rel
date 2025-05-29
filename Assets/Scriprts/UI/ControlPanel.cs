using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private CameraController cameraController;

    private bool isPaused = false;

    void Start()
    {
        // Назначаем обработчик кнопки
        closeButton.onClick.AddListener(ClosePanel);

        // Показываем панель при старте
        OpenPanel();

        // Находим контроллеры, если не назначены в инспекторе
        if (playerController == null)
            playerController = FindObjectOfType<FirstPersonController>();

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        // Открытие/закрытие по клавише H
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isPaused)
                ClosePanel();
            else
                OpenPanel();
        }
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        isPaused = true;

        // Активируем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Останавливаем игрока и камеру
        if (playerController != null)
            playerController.LockStatePlayer();

        if (cameraController != null)
            cameraController.enabled = false;
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        isPaused = false;

        // Деактивируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Возобновляем управление
        if (playerController != null)
            playerController.LockStatePlayer();

        if (cameraController != null)
            cameraController.enabled = true;
    }
}