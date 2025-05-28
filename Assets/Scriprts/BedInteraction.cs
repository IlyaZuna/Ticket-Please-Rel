using UnityEngine;
using UnityEngine.SceneManagement;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [Header("��������� ���")]
    public KeyCode sleepKey = KeyCode.E; // ������� ��� ��������� ���
    public string sleepText = "������� [E] ����� �������";
    public bool isTimeSkipping = true; // ���������� ����� ������ ������������?
    public float hoursToSkip = 8f; // ������� ����� ����������
    public GameObject sleepUI; // �����������: UI-���������

    private bool _playerInRange = false;

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(sleepKey))
        {
            Sleep();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            if (sleepUI != null) sleepUI.SetActive(true);
            Debug.Log(sleepText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            if (sleepUI != null) sleepUI.SetActive(false);
        }
    }

    public void Sleep()
    {


        // ������������ ����� (���� ����� "����� ����")
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        // �������������� ������� (����������, ���� � �.�.)
    }

    // ���������� ���������� IInteractable (���� ������������)
    public void Interact()
    {
        Sleep();
    }
}