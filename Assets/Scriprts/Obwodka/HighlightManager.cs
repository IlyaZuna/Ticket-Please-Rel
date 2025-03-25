using UnityEngine;

public class HighlightManager : MonoBehaviour {
    private Material highlightMaterial; // Материал для подсветки
    private GameObject lastHighlightedObject; // Последний подсвеченный объект

    [SerializeField] private Color outlineColor = Color.yellow; // Цвет подсветки
    [SerializeField] private float outlineWidth = 0.03f; // Ширина контура

    void Start() {
        // Инициализация материала для подсветки
        highlightMaterial = new Material(Shader.Find("Custom/HighlightShader"));
        highlightMaterial.SetColor("_OutlineColor", outlineColor);
        highlightMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }

    void Update() {
        // Создаем луч из позиции курсора
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Проверяем попадание луча в объект
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Renderer rend = hitObject.GetComponent<Renderer>();

            if (rend != null) // Если у объекта есть Renderer
            {
                // Если это новый объект
                if (hitObject != lastHighlightedObject)
                {
                    ResetHighlight(); // Сбрасываем подсветку предыдущего объекта

                    // Применяем материал подсветки
                    rend.material = highlightMaterial;
                    lastHighlightedObject = hitObject;
                }
                return;
            }
        }

        // Если луч ни во что не попал, сбрасываем подсветку
        ResetHighlight();
    }

    void ResetHighlight() {
        if (lastHighlightedObject != null)
        {
            Renderer rend = lastHighlightedObject.GetComponent<Renderer>();
            if (rend != null)
            {
                // Возвращаем стандартный материал
                rend.material = new Material(Shader.Find("Standard"));
            }
            lastHighlightedObject = null;
        }
    }
}