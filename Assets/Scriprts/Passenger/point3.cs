using UnityEngine;

public class Point3 : MonoBehaviour
{
    public int Index; // ������ �����
    public bool IsOccupied { get; private set; }

    public void Start()
    {
        IsOccupied = false;
    }

    public void Occupy()
    {
        //Debug.Log("NOy���������" + Index);
        IsOccupied = true;
    }

    public void Release()
    {
        //Debug.Log("���������" + Index);
        IsOccupied = false;
    }
}