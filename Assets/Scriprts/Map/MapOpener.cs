using UnityEngine;

public class MapOpener : MonoBehaviour, IInteractable
{
    private MapController mapController;

    void Start()
    {
        // ������� MapController � �����
        mapController = FindObjectOfType<MapController>();
        if (mapController == null)
        {
            Debug.LogError("MapController not found in scene!");
        }
    }

    public void Interact()
    {
        // ���� ����� �������, ��������� �
        if (mapController != null && !mapController.IsMapOpen())
        {
            mapController.OpenMap();
        }
    }
}