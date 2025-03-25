using UnityEngine;

public class HighlightManager : MonoBehaviour {
    private Material highlightMaterial; // �������� ��� ���������
    private GameObject lastHighlightedObject; // ��������� ������������ ������

    [SerializeField] private Color outlineColor = Color.yellow; // ���� ���������
    [SerializeField] private float outlineWidth = 0.03f; // ������ �������

    void Start() {
        // ������������� ��������� ��� ���������
        highlightMaterial = new Material(Shader.Find("Custom/HighlightShader"));
        highlightMaterial.SetColor("_OutlineColor", outlineColor);
        highlightMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }

    void Update() {
        // ������� ��� �� ������� �������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ��������� ��������� ���� � ������
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Renderer rend = hitObject.GetComponent<Renderer>();

            if (rend != null) // ���� � ������� ���� Renderer
            {
                // ���� ��� ����� ������
                if (hitObject != lastHighlightedObject)
                {
                    ResetHighlight(); // ���������� ��������� ����������� �������

                    // ��������� �������� ���������
                    rend.material = highlightMaterial;
                    lastHighlightedObject = hitObject;
                }
                return;
            }
        }

        // ���� ��� �� �� ��� �� �����, ���������� ���������
        ResetHighlight();
    }

    void ResetHighlight() {
        if (lastHighlightedObject != null)
        {
            Renderer rend = lastHighlightedObject.GetComponent<Renderer>();
            if (rend != null)
            {
                // ���������� ����������� ��������
                rend.material = new Material(Shader.Find("Standard"));
            }
            lastHighlightedObject = null;
        }
    }
}