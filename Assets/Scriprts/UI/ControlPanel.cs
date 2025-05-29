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
        // ��������� ���������� ������
        closeButton.onClick.AddListener(ClosePanel);

        // ���������� ������ ��� ������
        OpenPanel();

        // ������� �����������, ���� �� ��������� � ����������
        if (playerController == null)
            playerController = FindObjectOfType<FirstPersonController>();

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        // ��������/�������� �� ������� H
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

        // ���������� ������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ������������� ������ � ������
        if (playerController != null)
            playerController.LockStatePlayer();

        if (cameraController != null)
            cameraController.enabled = false;
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        isPaused = false;

        // ������������ ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ������������ ����������
        if (playerController != null)
            playerController.LockStatePlayer();

        if (cameraController != null)
            cameraController.enabled = true;
    }
}