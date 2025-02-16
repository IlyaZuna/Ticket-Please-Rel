using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWay : MonoBehaviour
{
    private Point[] points; // Массив точек
    private Point2[] points2; // Массив точек
    private Point3[] points3; // Массив точек   
    private bool CanINBus;
    private int summ = 0;
    void Awake()
    {
        points = FindObjectsOfType<Point>();
        System.Array.Sort(points, (a, b) => a.Index.CompareTo(b.Index));
        points2 = FindObjectsOfType<Point2>();
        System.Array.Sort(points2, (a, b) => a.Index.CompareTo(b.Index));
        points3 = FindObjectsOfType<Point3>();
        System.Array.Sort(points3, (a, b) => a.Index.CompareTo(b.Index));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ICanTMove() 
    {
        summ++;
        
    }
    public void ICanMove()
    {
        summ--;
       
    }
    public void WayExit(int RowExit,int index, out Transform target, out int RowExitOut)
    {
        if(RowExit != -1 && RowExit != -3)
        {
            points3[index].Release();
            Debug.Log("RowExit  на выходе " + RowExit);
            target = points2[RowExit].transform;
            RowExitOut = -1;
        }
        else if(RowExit == -1)
        {
            target = points[3].transform;
            RowExitOut = -3;
        }
        else 
        {
            target = points[0].transform;
            RowExitOut = -2;
        }
       

    }
    public void Way(int index, out Transform target, out int inde, out int RowExitOut)
    {
        if (summ == 0)
        {
            int nextindex = index + 1;
            if (nextindex == points.Length)
            {
                nextindex = index;
            }
            Debug.Log("Index  " + index);
            if (index <= points.Length - 1 && !points[nextindex].IsOccupied)
            {
                Debug.Log("Index 1 " + index);
                points[index].Release();
                inde = ++index;
                points[inde].Occupy();
                target = points[inde].transform;
                RowExitOut = -2;
                return;
            }
            else if (index == points.Length - 1)
            {
                Debug.Log("Index 2 " + index);
                points[index].Release();
                RowExitOut = -1;
                inde = index;
                target = points[inde].transform;
                return;
            }
            else
            {
                Debug.Log("Index 3" + index);
                RowExitOut = -2;
                inde = index;
                target = null; // target = points[inde].transform;
                return;
            }
        }
        else
        {
            inde = index;
            RowExitOut = -2;
            target = null;
        }
    }
    public void SetSeatRowTarget(int RowExit, int index, out Transform targetPoint, out int inde, out int RowExitOut)
    {
        targetPoint = null;
        

        index = Random.Range(0, 17);  // Присваиваем случайное значение, учитывая размер массива
        
        for (int i = 0; i < 36; i++)
        {
            if (!points3[index].IsOccupied)
            {
                switch (index)
                {
                    case >= 0 and <= 3:
                        RowExit = 0;
                        break;
                    case >= 4 and <= 7:
                        RowExit = 1;
                        break;
                    case >= 8 and <= 11:
                        RowExit = 2;
                        break;
                    case >= 12 and <= 15:
                        RowExit = 3;
                        break;
                    case >= 16 and <= 17:
                        RowExit = 4;
                        break;
                    default:
                        Debug.Log("Некорректный currentIndex в SetSeatRowMumberTarget!" + index);
                        break;
                }                
                points3[index].Occupy();
                targetPoint = points2[RowExit].transform;  // Устанавливаем цель               
                inde = index;
                RowExitOut = RowExit;
                Debug.Log("Вернул Row" + RowExit);
                return;
            }
            else
            {
                index++;
                if (index == points3.Length)
                {
                    index = 0;
                }
            }
        }
        inde = index;
        RowExitOut = RowExit;
    }

    public Transform Seatpoints(int index)
    {
        return points3[index].transform;
    }
}
