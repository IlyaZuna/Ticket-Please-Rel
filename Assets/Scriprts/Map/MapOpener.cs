using UnityEngine;

public class MapOpener : MonoBehaviour, IInteractable
{
    private MapController mapController;

    void Start()
    {
        // Находим MapController в сцене
        mapController = FindObjectOfType<MapController>();
        if (mapController == null)
        {
            Debug.LogError("MapController not found in scene!");
        }
    }

    public void Interact()
    {
        // Если карта закрыта, открываем её
        if (mapController != null && !mapController.IsMapOpen())
        {
            mapController.OpenMap();
        }
    }
}