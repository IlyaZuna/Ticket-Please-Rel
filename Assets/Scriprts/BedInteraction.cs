using UnityEngine;
using UnityEngine.SceneManagement;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [Header("Настройки сна")]
    public KeyCode sleepKey = KeyCode.E; // Клавиша для активации сна
    public string sleepText = "Нажмите [E] чтобы поспать";
    public bool isTimeSkipping = true; // Пропускать время вместо перезагрузки?
    public float hoursToSkip = 8f; // Сколько часов пропустить
    public GameObject sleepUI; // Опционально: UI-подсказка

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


        // Перезагрузка сцены (если нужно "новое утро")
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        // Дополнительные эффекты (затемнение, звук и т.д.)
    }

    // Реализация интерфейса IInteractable (если используется)
    public void Interact()
    {
        Sleep();
    }
}