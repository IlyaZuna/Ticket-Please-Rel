using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject mapCanvas; // Ссылка на MapCanvas
    [SerializeField] private RectTransform mapImage; // Ссылка на MapImage (RawImage карты)
    [SerializeField] private Button closeButton; // Кнопка закрытия карты
    [SerializeField] private RectTransform playerIcon; // Иконка игрока
    [SerializeField] private RectTransform busIcon; // Иконка автобуса
    [SerializeField] private RectTransform[] stopIcons; // Иконки остановок
    [SerializeField] private RectTransform[] buildingIcons; // Иконки зданий (заправка, мастерская и т.д.)
    [SerializeField] private TextMeshProUGUI[] buildingLabels; // Подписи зданий
    [SerializeField] private LineRenderer routeLine; // Линия маршрута от автобуса до ближайшей остановки

    private GameObject player; // Объект игрока
    private GameObject bus; // Объект автобуса
    private GameObject[] stops; // Все остановки
    private GameObject[] buildings; // Все здания
    [SerializeField] private float mapScaleX = 1f; // Масштаб по оси X (из Inspector)
    [SerializeField] private float mapScaleY = 1f; // Масштаб по оси Y (из Inspector)
    [SerializeField] private Vector2 mapOffset = new Vector2(200f, 300f); // Смещение карты (из Inspector)
    [SerializeField] private float markerRotation = 0f; // Угол поворота маркеров в градусах (из Inspector)
    [SerializeField] private bool autoAlignMap = false; // Флаг для автоматического выравнивания (включить/выключить)

    void Start()
    {
        // Изначально карта скрыта
        mapCanvas.SetActive(false);

        // Настраиваем кнопку закрытия
        closeButton.onClick.AddListener(CloseMap);

        // Находим объекты в сцене
        player = GameObject.FindGameObjectWithTag("Player");
        bus = GameObject.FindGameObjectWithTag("Bus");
        stops = GameObject.FindGameObjectsWithTag("BusStop");
        buildings = new GameObject[4];
        buildings[0] = GameObject.FindGameObjectWithTag("FuelStation");
        buildings[1] = GameObject.FindGameObjectWithTag("Workshop");
        buildings[2] = GameObject.FindGameObjectWithTag("House");
        buildings[3] = GameObject.FindGameObjectWithTag("Dispatch");

        // Проверяем, что всё найдено
        if (player == null) Debug.LogError("Player not found!");
        if (bus == null) Debug.LogError("Bus not found!");
        if (stops.Length != stopIcons.Length) Debug.LogWarning("Mismatch between stops (" + stops.Length + ") and stopIcons (" + stopIcons.Length + ")! Adjusting stopIcons to match stops.");
        if (buildings.Length != buildingIcons.Length || buildings.Length != buildingLabels.Length)
            Debug.LogError("Mismatch between buildings and buildingIcons/labels!");

        // Автоматически подгоняем размер массива stopIcons под количество stops
        if (stops.Length > 0 && stopIcons.Length < stops.Length)
        {
            System.Array.Resize(ref stopIcons, stops.Length);
            Debug.Log("Resized stopIcons to match " + stops.Length + " stops.");
        }

        // Автоматическое выравнивание mapOffset по остановкам (если включено)
        if (autoAlignMap)
        {
            AutoAlignMapOffset();
        }

        // Инициализируем линию маршрута
        routeLine.positionCount = 2;
        routeLine.startWidth = 5f;
        routeLine.endWidth = 5f;
        routeLine.startColor = Color.green; // Устанавливаем цвет линии
        routeLine.endColor = Color.green;
    }

    void Update()
    {
        // Открытие карты по клавише "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!mapCanvas.activeSelf)
                OpenMap();
            else
                CloseMap();
        }

        // Обновляем позиции объектов на карте, если карта открыта
        if (mapCanvas.activeSelf)
        {
            UpdateMap();
        }
    }

    void OpenMap()
    {
        mapCanvas.SetActive(true);
        Time.timeScale = 0f; // Ставим игру на паузу
        UpdateMap(); // Обновляем позиции сразу
        // Показываем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseMap()
    {
        mapCanvas.SetActive(false);
        Time.timeScale = 1f; // Возвращаем игру в нормальное состояние
        // Скрываем курсор (если нужно)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateMap()
    {
        // Обновляем позицию игрока
        UpdateIconPosition(player, playerIcon);

        // Обновляем позицию автобуса
        UpdateIconPosition(bus, busIcon);

        // Обновляем позиции остановок (только для доступных иконок)
        int stopCount = Mathf.Min(stops.Length, stopIcons.Length);
        for (int i = 0; i < stopCount; i++)
        {
            UpdateIconPosition(stops[i], stopIcons[i]);
        }

        // Обновляем позиции зданий
        for (int i = 0; i < buildings.Length; i++)
        {
            UpdateIconPosition(buildings[i], buildingIcons[i]);
        }

        // Обновляем маршрут до ближайшей остановки
        UpdateRoute();
    }

    void UpdateIconPosition(GameObject worldObject, RectTransform icon)
    {
        if (worldObject == null || icon == null) return;

        // Преобразуем мировую позицию в координаты карты с независимым масштабом по X и Y
        Vector2 worldPos = new Vector2(worldObject.transform.position.x, worldObject.transform.position.z);
        Vector2 mapPos = new Vector2(
            (worldPos.x - mapOffset.x) * mapScaleX,
            (worldPos.y - mapOffset.y) * mapScaleY
        );

        // Применяем вращение маркера вокруг центра карты
        mapPos = RotatePointAroundPivot(mapPos, Vector2.zero, markerRotation);

        // Устанавливаем позицию иконки
        icon.anchoredPosition = mapPos;
    }

    void UpdateRoute()
    {
        if (bus == null || stops.Length == 0) return;

        // Находим ближайшую остановку по маршруту (минимальный indexStop)
        GameObject nearestStop = null;
        int minIndex = int.MaxValue;
        foreach (var stop in stops)
        {
            BusStopTrigger stopTrigger = stop.GetComponent<BusStopTrigger>();
            if (stopTrigger != null && stopTrigger.indexStop < minIndex)
            {
                minIndex = stopTrigger.indexStop;
                nearestStop = stop;
            }
        }

        if (nearestStop == null) return;

        // Преобразуем позиции автобуса и остановки в координаты карты
        Vector2 busWorldPos = new Vector2(bus.transform.position.x, bus.transform.position.z);
        Vector2 stopWorldPos = new Vector2(nearestStop.transform.position.x, nearestStop.transform.position.z);
        Vector2 busMapPos = new Vector2(
            (busWorldPos.x - mapOffset.x) * mapScaleX,
            (busWorldPos.y - mapOffset.y) * mapScaleY
        );
        Vector2 stopMapPos = new Vector2(
            (stopWorldPos.x - mapOffset.x) * mapScaleX,
            (stopWorldPos.y - mapOffset.y) * mapScaleY
        );

        // Применяем вращение к позициям маршрута
        busMapPos = RotatePointAroundPivot(busMapPos, Vector2.zero, markerRotation);
        stopMapPos = RotatePointAroundPivot(stopMapPos, Vector2.zero, markerRotation);

        // Устанавливаем позиции линии маршрута
        routeLine.SetPosition(0, new Vector3(busMapPos.x, busMapPos.y, -1f)); // Начало (автобус)
        routeLine.SetPosition(1, new Vector3(stopMapPos.x, stopMapPos.y, -1f)); // Конец (остановка)
    }

    Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        // Переводим угол в радианы
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        // Смещаем точку относительно центра вращения
        Vector2 translatedPoint = point - pivot;
        // Применяем вращение
        float xNew = translatedPoint.x * cos - translatedPoint.y * sin;
        float yNew = translatedPoint.x * sin + translatedPoint.y * cos;
        // Возвращаем точку обратно
        return new Vector2(xNew, yNew) + pivot;
    }

    void AutoAlignMapOffset()
    {
        if (stops.Length > 0)
        {
            // Вычисляем среднюю позицию всех остановок для автоматического выравнивания
            Vector2 avgPos = Vector2.zero;
            foreach (var stop in stops)
            {
                Vector2 stopPos = new Vector2(stop.transform.position.x, stop.transform.position.z);
                avgPos += stopPos;
            }
            avgPos /= stops.Length;
            mapOffset = avgPos; // Устанавливаем центр карты как среднюю позицию остановок
            Debug.Log("Auto-aligned mapOffset to: " + mapOffset);
        }
    }

    // Публичный метод для проверки, открыта ли карта
    public bool IsMapOpen()
    {
        return mapCanvas.activeSelf;
    }
}