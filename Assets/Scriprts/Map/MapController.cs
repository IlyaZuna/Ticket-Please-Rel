using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject mapCanvas; // ������ �� MapCanvas
    [SerializeField] private RectTransform mapImage; // ������ �� MapImage (RawImage �����)
    [SerializeField] private Button closeButton; // ������ �������� �����
    [SerializeField] private RectTransform playerIcon; // ������ ������
    [SerializeField] private RectTransform busIcon; // ������ ��������
    [SerializeField] private RectTransform[] stopIcons; // ������ ���������
    [SerializeField] private RectTransform[] buildingIcons; // ������ ������ (��������, ���������� � �.�.)
    [SerializeField] private TextMeshProUGUI[] buildingLabels; // ������� ������

    private GameObject player; // ������ ������
    private GameObject bus; // ������ ��������
    private GameObject[] stops; // ��� ���������
    private GameObject[] buildings; // ��� ������
    [SerializeField] private float mapScaleX = 1f; // ������� �� ��� X (�� Inspector)
    [SerializeField] private float mapScaleY = 1f; // ������� �� ��� Y (�� Inspector)
    [SerializeField] private Vector2 mapOffset = new Vector2(200f, 300f); // �������� ����� (�� Inspector)
    [SerializeField] private float markerRotation = 0f; // ���� �������� �������� � �������� (�� Inspector)
    [SerializeField] private bool autoAlignMap = false; // ���� ��� ��������������� ������������ (��������/���������)

    void Start()
    {
        // ���������� ����� ������
        mapCanvas.SetActive(false);

        // ����������� ������ ��������
        closeButton.onClick.AddListener(CloseMap);

        // ������� ������� � �����
        player = GameObject.FindGameObjectWithTag("Player");
        bus = GameObject.FindGameObjectWithTag("Bus");
        stops = GameObject.FindGameObjectsWithTag("BusStop");
        buildings = new GameObject[4];
        buildings[0] = GameObject.FindGameObjectWithTag("FuelStation");
        buildings[1] = GameObject.FindGameObjectWithTag("Workshop");
        buildings[2] = GameObject.FindGameObjectWithTag("House");
        buildings[3] = GameObject.FindGameObjectWithTag("Dispatch");

        // ���������, ��� �� �������
        if (player == null) Debug.LogError("Player not found!");
        if (bus == null) Debug.LogError("Bus not found!");
        if (stops.Length != stopIcons.Length) Debug.LogWarning("Mismatch between stops (" + stops.Length + ") and stopIcons (" + stopIcons.Length + ")! Adjusting stopIcons to match stops.");
        if (buildings.Length != buildingIcons.Length || buildings.Length != buildingLabels.Length)
            Debug.LogError("Mismatch between buildings and buildingIcons/labels!");

        // ������������� ��������� ������ ������� stopIcons ��� ���������� stops
        if (stops.Length > 0 && stopIcons.Length < stops.Length)
        {
            System.Array.Resize(ref stopIcons, stops.Length);
            Debug.Log("Resized stopIcons to match " + stops.Length + " stops.");
        }

        // �������������� ������������ mapOffset �� ���������� (���� ��������)
        if (autoAlignMap)
        {
            AutoAlignMapOffset();
        }
    }

    void Update()
    {
        // ��������/�������� ����� �� ������� "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!mapCanvas.activeSelf)
                OpenMap();
            else
                CloseMap();
        }

        // ��������� ������� �������� �� �����, ���� ����� �������
        if (mapCanvas.activeSelf)
        {
            UpdateMap();
        }
    }

    // ��������� ����� ��� �������� ����� (������������ �������� M � ����� ��������������)
    public void OpenMap()
    {
        mapCanvas.SetActive(true);
        Time.timeScale = 0f; // ������ ���� �� �����
        UpdateMap(); // ��������� ������� �����
        // ���������� ������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseMap()
    {
        mapCanvas.SetActive(false);
        Time.timeScale = 1f; // ���������� ���� � ���������� ���������
        // �������� ������ (���� �����)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateMap()
    {
        // ��������� ������� ������
        UpdateIconPosition(player, playerIcon);

        // ��������� ������� ��������
        UpdateIconPosition(bus, busIcon);

        // ��������� ������� ��������� (������ ��� ��������� ������)
        int stopCount = Mathf.Min(stops.Length, stopIcons.Length);
        for (int i = 0; i < stopCount; i++)
        {
            UpdateIconPosition(stops[i], stopIcons[i]);
        }

        // ��������� ������� ������
        for (int i = 0; i < buildings.Length; i++)
        {
            UpdateIconPosition(buildings[i], buildingIcons[i]);
        }
    }

    void UpdateIconPosition(GameObject worldObject, RectTransform icon)
    {
        if (worldObject == null || icon == null) return;

        // ����������� ������� ������� � ���������� ����� � ����������� ��������� �� X � Y
        Vector2 worldPos = new Vector2(worldObject.transform.position.x, worldObject.transform.position.z);
        Vector2 mapPos = new Vector2(
            (worldPos.x - mapOffset.x) * mapScaleX,
            (worldPos.y - mapOffset.y) * mapScaleY
        );

        // ��������� �������� ������� ������ ������ �����
        mapPos = RotatePointAroundPivot(mapPos, Vector2.zero, markerRotation);

        // ������������� ������� ������
        icon.anchoredPosition = mapPos;
    }

    Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        // ��������� ���� � �������
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        // ������� ����� ������������ ������ ��������
        Vector2 translatedPoint = point - pivot;
        // ��������� ��������
        float xNew = translatedPoint.x * cos - translatedPoint.y * sin;
        float yNew = translatedPoint.x * sin + translatedPoint.y * cos;
        // ���������� ����� �������
        return new Vector2(xNew, yNew) + pivot;
    }

    void AutoAlignMapOffset()
    {
        if (stops.Length > 0)
        {
            // ��������� ������� ������� ���� ��������� ��� ��������������� ������������
            Vector2 avgPos = Vector2.zero;
            foreach (var stop in stops)
            {
                Vector2 stopPos = new Vector2(stop.transform.position.x, stop.transform.position.z);
                avgPos += stopPos;
            }
            avgPos /= stops.Length;
            mapOffset = avgPos; // ������������� ����� ����� ��� ������� ������� ���������
            Debug.Log("Auto-aligned mapOffset to: " + mapOffset);
        }
    }

    // ��������� ����� ��� ��������, ������� �� �����
    public bool IsMapOpen()
    {
        return mapCanvas.activeSelf;
    }
}