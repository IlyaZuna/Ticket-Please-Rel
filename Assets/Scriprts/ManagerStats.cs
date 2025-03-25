using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStats : MonoBehaviour
{
    [SerializeField]private int _PasengerScore = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addPasengerScore()
    {
        _PasengerScore++;
    }
    public int GetPasengrScore()
    {
        return _PasengerScore;
    }
    public bool CheckQwest(int Qwest)
    {
        
        if (Qwest >= _PasengerScore)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
