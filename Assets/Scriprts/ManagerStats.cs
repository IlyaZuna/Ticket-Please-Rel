using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStats : MonoBehaviour
{
    public int _pasengerallSkore;
    public int _pasengerSkore;
    public int day;
    private DriverIncome driverIncome;
    public int allincame;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addPasengerSkore()
    {
        _pasengerSkore++;
        return;
    }
    public void addPasengerallSkore()
    {
        _pasengerallSkore++;
        return;
    }
    public void addDay()
    {
        day++;
        addPasengerallSkore();
        allincame = driverIncome.Incame();
        return;
    }
    public int getPasengerSkore()
    {
        return _pasengerSkore;
    }

}
