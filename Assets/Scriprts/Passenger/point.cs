using UnityEngine;

public class Point : MonoBehaviour
{
    public int Index; // ������ �����
    public bool IsOccupied { get; private set; }

    public void Start()
    {
        bool IsOccupied = false;
    }

    public void Occupy()
    {
        //Debug.Log("NOy���������"+ Index);
        IsOccupied = true;
    }

    public void Release()
    {
        //Debug.Log("���������"+ Index);
        IsOccupied = false;
    }
}