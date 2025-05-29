using UnityEngine;
using TMPro;

public class FloatingHint : MonoBehaviour
{
    [SerializeField] private string hintText;
    [SerializeField] private float heightAboveObject = 1.5f;
    [SerializeField] private GameObject hintPrefab;
    [SerializeField] private Vector3 offset = new Vector3(0, 0.2f, 0);

    private GameObject hintInstance;
    private TextMeshPro hintTextMesh;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (hintPrefab != null)
        {
            // Создаем экземпляр подсказки
            hintInstance = Instantiate(hintPrefab, transform.position, Quaternion.identity);
            hintInstance.transform.SetParent(transform);
            hintInstance.SetActive(false);

            // Получаем компонент текста
            hintTextMesh = hintInstance.GetComponentInChildren<TextMeshPro>();
            if (hintTextMesh != null)
            {
                hintTextMesh.text = hintText;
            }
        }
    }

    public void ShowHint()
    {
        if (hintInstance != null)
        {
            hintInstance.SetActive(true);
            UpdatePosition();
        }
    }

    public void HideHint()
    {
        if (hintInstance != null)
        {
            hintInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (hintInstance != null && hintInstance.activeSelf)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        // Позиционируем подсказку над объектом
        Vector3 worldPosition = transform.position + Vector3.up * heightAboveObject + offset;
        hintInstance.transform.position = worldPosition;

        // Поворачиваем подсказку к камере
        hintInstance.transform.LookAt(mainCamera.transform);
        hintInstance.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }

    void OnDestroy()
    {
        if (hintInstance != null)
        {
            Destroy(hintInstance);
        }
    }
}