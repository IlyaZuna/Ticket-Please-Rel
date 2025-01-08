using UnityEngine;

public class Point : MonoBehaviour
{
    public int Index; // Индекс точки
    public bool IsOccupied { get; private set; }

    public void Start()
    {
        bool IsOccupied = false;
    }

    public void Occupy()
    {
        //Debug.Log("NOyосвободил"+ Index);
        IsOccupied = true;
    }

    public void Release()
    {
        //Debug.Log("освободил"+ Index);
        IsOccupied = false;
    }
}