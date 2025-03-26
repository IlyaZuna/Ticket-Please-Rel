using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStats : MonoBehaviour
{

    public int _pasengerSkore;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CheckQwest(int Qwest)
    {
        if(Qwest <= _pasengerSkore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void addPasengerSkore()
    {
        _pasengerSkore++;
        return;
    }
    public int getPasengerSkore() {  return _pasengerSkore; }
}
